using System.Data;
using HolosMigratorUI.Core;
using System.ComponentModel;

namespace HolosMigratorUI.UI;

public partial class DashboardUserControl : UserControl
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private readonly Func<string> _hostProvider;
    private readonly Func<int> _sshPortProvider;
    private readonly Func<string> _serverUserProvider;
    private readonly Func<string> _sshAuthModeProvider;
    private readonly Func<string> _sshKeyPathProvider;
    private readonly Func<string> _sshPasswordProvider;
    private readonly System.Windows.Forms.Timer _refreshTimer = new() { Interval = 30000 };
    private bool _isRefreshing;

    public DashboardUserControl()
        : this(
            () => string.Empty,
            () => 22,
            () => string.Empty,
            () => "Auto",
            () => string.Empty,
            () => string.Empty)
    {
    }

    public DashboardUserControl(
        Func<string> hostProvider,
        Func<int> sshPortProvider,
        Func<string> serverUserProvider,
        Func<string> sshAuthModeProvider,
        Func<string> sshKeyPathProvider,
        Func<string> sshPasswordProvider)
    {
        _hostProvider = hostProvider;
        _sshPortProvider = sshPortProvider;
        _serverUserProvider = serverUserProvider;
        _sshAuthModeProvider = sshAuthModeProvider;
        _sshKeyPathProvider = sshKeyPathProvider;
        _sshPasswordProvider = sshPasswordProvider;

        InitializeComponent();
        UIHelper.AplicarEstiloModernoTabla(_gridRecentRuns);

        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            return;
        }

        _btnRefresh.Click += async (_, _) => await RefreshDashboardAsync();
        _refreshTimer.Tick += async (_, _) => await RefreshDashboardAsync();
        _refreshTimer.Start();

        Load += async (_, _) => await RefreshDashboardAsync();
        HandleDestroyed += (_, _) => _refreshTimer.Stop();
    }

    private static string NormalizeServiceState(string? state)
    {
        if (string.Equals(state, "running", StringComparison.OrdinalIgnoreCase))
        {
            return "running";
        }

        if (string.Equals(state, "stopped", StringComparison.OrdinalIgnoreCase))
        {
            return "stopped";
        }

        return "unknown";
    }

    private static void SetStatusLabel(Label label, string state, string upText, string downText)
    {
        var normalized = NormalizeServiceState(state);

        if (normalized == "running")
        {
            label.Text = upText;
            label.ForeColor = Color.FromArgb(78, 214, 138);
            return;
        }

        if (normalized == "stopped")
        {
            label.Text = downText;
            label.ForeColor = Color.FromArgb(255, 129, 129);
            return;
        }

        label.Text = "Estado no verificado";
        label.ForeColor = Color.FromArgb(255, 209, 102);
    }

    private static string ResolveServiceState(string service, IReadOnlyDictionary<string, string>? remoteStates, string localState)
    {
        if (remoteStates != null && remoteStates.TryGetValue(service, out var remoteState))
        {
            return NormalizeServiceState(remoteState);
        }

        return NormalizeServiceState(localState);
    }

    private static string GetMetricValue(IReadOnlyDictionary<string, string>? metrics, string key, string fallback)
    {
        if (metrics != null && metrics.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        return fallback;
    }

    public async Task RefreshDashboardAsync()
    {
        if (_isRefreshing)
        {
            return;
        }

        _isRefreshing = true;
        try
        {
            var host = _hostProvider().Trim();
            var port = _sshPortProvider();
            var serverUser = _serverUserProvider().Trim();
            var sshKeyPath = _sshKeyPathProvider().Trim();
            var sshPassword = _sshPasswordProvider().Trim();
            var networkReachable = !string.IsNullOrWhiteSpace(host) && await HealthCheckService.IsHostReachableAsync(host, port);
            var snapshot = _state.GetLatestHealthSnapshot(host) with { HostReachable = networkReachable, CheckedAt = DateTime.Now };

            IReadOnlyDictionary<string, string>? remoteStates = null;
            IReadOnlyDictionary<string, string>? remoteMetrics = null;
            string stateSource;
            string resourcesSource;

            var canUseRemoteDockerCheck =
                snapshot.HostReachable
                && !string.IsNullOrWhiteSpace(serverUser)
                && (!string.IsNullOrWhiteSpace(sshKeyPath) || !string.IsNullOrWhiteSpace(sshPassword));

            if (canUseRemoteDockerCheck)
            {
                remoteStates = await HealthCheckService.TryGetDockerServiceStatesAsync(
                    host,
                    serverUser,
                    port,
                    string.IsNullOrWhiteSpace(sshKeyPath) ? null : sshKeyPath,
                    string.IsNullOrWhiteSpace(sshPassword) ? null : sshPassword);

                remoteMetrics = await HealthCheckService.TryGetServerResourceMetricsAsync(
                    host,
                    serverUser,
                    port,
                    string.IsNullOrWhiteSpace(sshKeyPath) ? null : sshKeyPath,
                    string.IsNullOrWhiteSpace(sshPassword) ? null : sshPassword);

                if (remoteStates != null)
                {
                    foreach (var entry in remoteStates)
                    {
                        _state.SetServiceState(entry.Key, entry.Value);
                    }
                }
            }

            if (!snapshot.HostReachable)
            {
                stateSource = "Fuente: verificacion TCP local (sin conexion SSH)";
                resourcesSource = "Recursos: sin conexion SSH";
            }
            else if (remoteStates != null)
            {
                stateSource = "Fuente: SSH remoto (docker ps)";
                resourcesSource = remoteMetrics != null
                    ? "Recursos: SSH remoto (/proc + free + df)"
                    : "Recursos: sin datos remotos";
            }
            else if (!canUseRemoteDockerCheck)
            {
                stateSource = "Fuente: telemetria local (sin credenciales SSH configuradas)";
                resourcesSource = "Recursos: configura usuario y password o llave SSH";
            }
            else
            {
                stateSource = "Fuente: telemetria local (sin datos remotos de contenedores)";
                resourcesSource = remoteMetrics != null
                    ? "Recursos: SSH remoto (/proc + free + df)"
                    : "Recursos: sin datos remotos";
            }

            var sqlState = ResolveServiceState("sql", remoteStates, _state.GetServiceState("sql"));
            var apiState = ResolveServiceState("api", remoteStates, _state.GetServiceState("api"));
            var frontState = ResolveServiceState("front", remoteStates, _state.GetServiceState("front"));

            if (string.IsNullOrWhiteSpace(host))
            {
                _lblHostStatus.Text = "Host no configurado";
                _lblHostStatus.ForeColor = Color.FromArgb(255, 209, 102);
                _lblStateSource.Text = "Fuente: host no configurado";

                _lblCpuUsage.Text = "--";
                _lblMemoryUsage.Text = "--";
                _lblDiskUsage.Text = "--";
                _lblUptime.Text = "--";
            }
            else
            {
                if (snapshot.HostReachable)
                {
                    _lblHostStatus.Text = $"En linea ({host}:{port})";
                    _lblHostStatus.ForeColor = Color.FromArgb(78, 214, 138);
                }
                else
                {
                    _lblHostStatus.Text = $"Sin conexion ({host}:{port})";
                    _lblHostStatus.ForeColor = Color.FromArgb(255, 129, 129);
                }

                _lblStateSource.Text = stateSource + " | " + resourcesSource;

                var unavailableReason = !snapshot.HostReachable
                    ? "Sin conexion SSH"
                    : (canUseRemoteDockerCheck ? "Sin datos remotos" : "Requiere llave SSH");

                _lblCpuUsage.Text = GetMetricValue(remoteMetrics, "cpu", unavailableReason);
                _lblMemoryUsage.Text = GetMetricValue(remoteMetrics, "memory", unavailableReason);
                _lblDiskUsage.Text = GetMetricValue(remoteMetrics, "disk", unavailableReason);
                _lblUptime.Text = GetMetricValue(remoteMetrics, "uptime", unavailableReason);

                _lblCpuUsage.ForeColor = Color.FromArgb(120, 214, 255);
                _lblMemoryUsage.ForeColor = Color.FromArgb(188, 158, 255);
                _lblDiskUsage.ForeColor = Color.FromArgb(130, 198, 255);
                _lblUptime.ForeColor = Color.FromArgb(255, 209, 102);
            }

            SetStatusLabel(_lblSqlStatus, sqlState, "Servicio activo", "Servicio detenido");
            SetStatusLabel(_lblApiStatus, apiState, "Servicio activo", "Servicio detenido");
            SetStatusLabel(_lblFrontStatus, frontState, "Servicio activo", "Servicio detenido");

            var serviceStates = new[] { sqlState, apiState, frontState };
            var activeServices = serviceStates.Count(s => s == "running");
            var stoppedServices = serviceStates.Count(s => s == "stopped");
            if (!snapshot.HostReachable)
            {
                _lblOverallStatus.Text = "Estado VPS: SIN CONEXION SSH";
                _lblOverallStatus.ForeColor = Color.FromArgb(255, 129, 129);
            }
            else if (activeServices == 3 && stoppedServices == 0)
            {
                _lblOverallStatus.Text = "Estado VPS: OPERATIVO";
                _lblOverallStatus.ForeColor = Color.FromArgb(78, 214, 138);
            }
            else if (stoppedServices > 0)
            {
                _lblOverallStatus.Text = $"Estado VPS: DEGRADADO ({activeServices}/3 servicios activos)";
                _lblOverallStatus.ForeColor = Color.FromArgb(255, 188, 94);
            }
            else
            {
                _lblOverallStatus.Text = "Estado VPS: CONECTADO, ESTADO DE SERVICIOS NO VERIFICADO";
                _lblOverallStatus.ForeColor = Color.FromArgb(255, 209, 102);
            }

            _lblLastChecked.Text = "Ultima verificacion: " + snapshot.CheckedAt.ToString("yyyy-MM-dd HH:mm:ss");

            var (successRate7d, successRate30d, mttrMinutes, avgDurationMinutes) = _state.GetKpis();
            _lblSuccess7d.Text = successRate7d > 0 ? $"{successRate7d:F1}%" : "--";
            _lblSuccess30d.Text = successRate30d > 0 ? $"{successRate30d:F1}%" : "--";
            _lblMttr.Text = mttrMinutes > 0 ? $"{mttrMinutes:F1} min" : "--";
            _lblAvgDuration.Text = avgDurationMinutes > 0 ? $"{avgDurationMinutes:F1} min" : "--";

            var table = new DataTable();
            table.Columns.Add("Inicio");
            table.Columns.Add("Fin");
            table.Columns.Add("Duracion");
            table.Columns.Add("Accion");
            table.Columns.Add("Entorno");
            table.Columns.Add("Exit");
            table.Columns.Add("Resultado");

            foreach (var run in _state.GetRuns().OrderByDescending(r => r.EndedAt).Take(20))
            {
                table.Rows.Add(
                    run.StartedAt.ToString("MM-dd HH:mm"),
                    run.EndedAt.ToString("MM-dd HH:mm"),
                    $"{run.Duration.TotalMinutes:F1}m",
                    run.Action,
                    run.Environment,
                    run.ExitCode,
                    run.Succeeded ? "OK" : "FAIL");
            }
            _gridRecentRuns.DataSource = table;
        }
        catch (Exception ex)
        {
            AppStateStore.Instance.AddLog(new LogEntry(
                DateTime.Now,
                "Dashboard",
                string.Empty,
                $"Error en RefreshDashboardAsync: {ex.Message}",
                LogSeverity.Error));
        }
        finally
        {
            _isRefreshing = false;
        }
    }
}