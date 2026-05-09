using System.Data;
using HolosMigratorUI.Core;
using System.Text;

namespace HolosMigratorUI.UI;

public class DashboardUserControl : UserControl
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private readonly Func<string> _hostProvider;
    private readonly Func<int> _sshPortProvider;
    private readonly Func<string> _serverUserProvider;
    private readonly Func<string> _sshAuthModeProvider;
    private readonly Func<string> _sshKeyPathProvider;
    private readonly System.Windows.Forms.Timer _refreshTimer = new() { Interval = 30000 };
    private bool _isRefreshing;

    private Label _lblOverallStatus = new();
    private Label _lblHostStatus = new();
    private Label _lblSqlStatus = new();
    private Label _lblApiStatus = new();
    private Label _lblFrontStatus = new();
    private Label _lblLastChecked = new();
    private Label _lblStateSource = new();
    private Label _lblCpuUsage = new();
    private Label _lblMemoryUsage = new();
    private Label _lblDiskUsage = new();
    private Label _lblUptime = new();
    private Label _lblSuccess7d = new();
    private Label _lblSuccess30d = new();
    private Label _lblMttr = new();
    private Label _lblAvgDuration = new();
    private DataGridView _gridRecentRuns = new();

    public DashboardUserControl(
        Func<string> hostProvider,
        Func<int> sshPortProvider,
        Func<string> serverUserProvider,
        Func<string> sshAuthModeProvider,
        Func<string> sshKeyPathProvider)
    {
        _hostProvider = hostProvider;
        _sshPortProvider = sshPortProvider;
        _serverUserProvider = serverUserProvider;
        _sshAuthModeProvider = sshAuthModeProvider;
        _sshKeyPathProvider = sshKeyPathProvider;

        BackColor = Color.FromArgb(10, 14, 22);
        ForeColor = Color.FromArgb(220, 230, 240);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = BackColor,
            Padding = new Padding(22, 16, 22, 22),
            ColumnCount = 1,
            RowCount = 5
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var pnlHeader = BuildHeaderPanel();
        var pnlOverall = BuildOverallStatusPanel();
        var pnlResourceCards = BuildResourceCardsPanel();
        var pnlServiceCards = BuildServiceCardsPanel();
        var pnlBottom = BuildBottomPanel();

        root.Controls.Add(pnlHeader, 0, 0);
        root.Controls.Add(pnlOverall, 0, 1);
        root.Controls.Add(pnlResourceCards, 0, 2);
        root.Controls.Add(pnlServiceCards, 0, 3);
        root.Controls.Add(pnlBottom, 0, 4);

        Controls.Add(root);

        _refreshTimer.Tick += async (_, _) => await RefreshDashboardAsync();
        _refreshTimer.Start();

        Load += async (_, _) => await RefreshDashboardAsync();
        HandleDestroyed += (_, _) => _refreshTimer.Stop();
    }

    private Control BuildHeaderPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = new Padding(0)
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));

        var titlePanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 6, 0, 0),
            Margin = new Padding(0)
        };

        var lblSubtitle = new Label
        {
            Text = "Estado en tiempo real de conectividad y servicios principales",
            Dock = DockStyle.Top,
            Height = 24,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
            ForeColor = Color.FromArgb(170, 189, 210),
            Margin = new Padding(0),
            AutoEllipsis = true
        };

        var lblTitle = new Label
        {
            Text = "CONTROL OPERATIVO DEL VPS",
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(236, 243, 251),
            Margin = new Padding(0)
        };

        titlePanel.Controls.Add(lblTitle);
        titlePanel.Controls.Add(lblSubtitle);

        var btnRefresh = new Button
        {
            Text = "Actualizar",
            Height = 40,
            Width = 140,
            Dock = DockStyle.Right,
            Margin = new Padding(0, 8, 0, 8),
            BackColor = Color.FromArgb(20, 104, 163),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        };
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.Click += async (_, _) => await RefreshDashboardAsync();

        panel.Controls.Add(titlePanel, 0, 0);
        panel.Controls.Add(btnRefresh, 1, 0);
        return panel;
    }

    private Control BuildOverallStatusPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(16, 24, 34),
            Padding = new Padding(16, 8, 16, 8)
        };

        _lblOverallStatus = new Label
        {
            Text = "Estado VPS: Pendiente de verificacion",
            Dock = DockStyle.Top,
            Height = 34,
            Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
            ForeColor = Color.FromArgb(255, 209, 102),
            AutoEllipsis = true
        };

        _lblLastChecked = new Label
        {
            Text = "Ultima verificacion: --",
            Dock = DockStyle.Top,
            Height = 22,
            Font = new Font("Segoe UI", 10F, FontStyle.Regular),
            ForeColor = Color.FromArgb(163, 180, 200)
        };

        _lblStateSource = new Label
        {
            Text = "Fuente: --",
            Dock = DockStyle.Top,
            Height = 20,
            Font = new Font("Segoe UI", 9F, FontStyle.Regular),
            ForeColor = Color.FromArgb(137, 154, 175),
            AutoEllipsis = true
        };

        panel.Controls.Add(_lblStateSource);
        panel.Controls.Add(_lblLastChecked);
        panel.Controls.Add(_lblOverallStatus);
        return panel;
    }

    private Control BuildServiceCardsPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 6, 0, 6)
        };

        for (var i = 0; i < 4; i++)
        {
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        }

        panel.Controls.Add(CreateMetricCard("HOST / SSH", out _lblHostStatus), 0, 0);
        panel.Controls.Add(CreateMetricCard("SQL", out _lblSqlStatus), 1, 0);
        panel.Controls.Add(CreateMetricCard("API", out _lblApiStatus), 2, 0);
        panel.Controls.Add(CreateMetricCard("FRONT", out _lblFrontStatus), 3, 0);
        return panel;
    }

    private Control BuildResourceCardsPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 6, 0, 6)
        };

        for (var i = 0; i < 4; i++)
        {
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        }

        panel.Controls.Add(CreateMetricCard("CPU VPS", out _lblCpuUsage), 0, 0);
        panel.Controls.Add(CreateMetricCard("MEMORIA VPS", out _lblMemoryUsage), 1, 0);
        panel.Controls.Add(CreateMetricCard("DISCO /", out _lblDiskUsage), 2, 0);
        panel.Controls.Add(CreateMetricCard("UPTIME", out _lblUptime), 3, 0);
        return panel;
    }

    private Control BuildBottomPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent,
            Padding = new Padding(0)
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var kpiPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 0, 0, 8)
        };

        for (var i = 0; i < 4; i++)
        {
            kpiPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        }

        kpiPanel.Controls.Add(CreateMetricCard("EXITO 7D", out _lblSuccess7d), 0, 0);
        kpiPanel.Controls.Add(CreateMetricCard("EXITO 30D", out _lblSuccess30d), 1, 0);
        kpiPanel.Controls.Add(CreateMetricCard("MTTR", out _lblMttr), 2, 0);
        kpiPanel.Controls.Add(CreateMetricCard("DURACION PROM.", out _lblAvgDuration), 3, 0);

        var runsContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(14, 20, 30),
            Padding = new Padding(10)
        };

        var runsTitle = new Label
        {
            Text = "EJECUCIONES RECIENTES",
            Dock = DockStyle.Top,
            Height = 30,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(170, 200, 235)
        };

        _gridRecentRuns = new DataGridView { Dock = DockStyle.Fill };
        UIHelper.AplicarEstiloModernoTabla(_gridRecentRuns);
        runsContainer.Controls.Add(_gridRecentRuns);
        runsContainer.Controls.Add(runsTitle);

        panel.Controls.Add(kpiPanel, 0, 0);
        panel.Controls.Add(runsContainer, 0, 1);
        return panel;
    }

    private static Panel CreateMetricCard(string title, out Label valueLabel)
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(22, 30, 43),
            Margin = new Padding(6),
            Padding = new Padding(12, 10, 12, 10)
        };

        var lblTitle = new Label
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 24,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(136, 157, 180)
        };

        valueLabel = new Label
        {
            Text = "--",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(229, 236, 244),
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        };

        card.Controls.Add(valueLabel);
        card.Controls.Add(lblTitle);
        return card;
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
            var sshAuthMode = _sshAuthModeProvider().Trim();
            var sshKeyPath = _sshKeyPathProvider().Trim();
            var networkReachable = !string.IsNullOrWhiteSpace(host) && await HealthCheckService.IsHostReachableAsync(host, port);
            var snapshot = _state.GetLatestHealthSnapshot(host) with { HostReachable = networkReachable, CheckedAt = DateTime.Now };

            IReadOnlyDictionary<string, string>? remoteStates = null;
            IReadOnlyDictionary<string, string>? remoteMetrics = null;
            string stateSource;
            string resourcesSource;

            var canUseRemoteDockerCheck =
                snapshot.HostReachable
                && !string.IsNullOrWhiteSpace(serverUser)
                && (!sshAuthMode.Equals("Password", StringComparison.OrdinalIgnoreCase)
                    || !string.IsNullOrWhiteSpace(sshKeyPath));

            if (canUseRemoteDockerCheck)
            {
                remoteStates = await HealthCheckService.TryGetDockerServiceStatesAsync(
                    host,
                    serverUser,
                    port,
                    string.IsNullOrWhiteSpace(sshKeyPath) ? null : sshKeyPath);

                remoteMetrics = await HealthCheckService.TryGetServerResourceMetricsAsync(
                    host,
                    serverUser,
                    port,
                    string.IsNullOrWhiteSpace(sshKeyPath) ? null : sshKeyPath);

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
            else if (!canUseRemoteDockerCheck && sshAuthMode.Equals("Password", StringComparison.OrdinalIgnoreCase))
            {
                stateSource = "Fuente: telemetria local (modo Password no permite chequeo en segundo plano)";
                resourcesSource = "Recursos: requiere llave SSH para chequeo en segundo plano";
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
        finally
        {
            _isRefreshing = false;
        }
    }
}

public class LogCenterUserControl : UserControl
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private DataGridView _gridLogs = new();
    private TextBox _txtSearch = new() { Width = 250 };
    private ComboBox _cmbSeverity = new() { Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
    
    public LogCenterUserControl()
    {
        BackColor = Color.FromArgb(10, 14, 22);
        ForeColor = Color.FromArgb(220, 230, 240);
        
        _gridLogs.Dock = DockStyle.Fill;
        UIHelper.AplicarEstiloModernoTabla(_gridLogs);

        var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(16, 24, 34) };
        _cmbSeverity.Items.AddRange(["All", "Info", "Warning", "Error", "Success"]);
        _cmbSeverity.SelectedIndex = 0;
        _cmbSeverity.BackColor = Color.FromArgb(17, 24, 34);
        _cmbSeverity.ForeColor = Color.FromArgb(226, 233, 241);

        _txtSearch.PlaceholderText = "Buscar en logs...";
        _txtSearch.BackColor = Color.FromArgb(17, 24, 34);
        _txtSearch.ForeColor = Color.FromArgb(226, 233, 241);
        _txtSearch.BorderStyle = BorderStyle.FixedSingle;
        
        var btnRef = new Button { Text = "Refresh", FlatStyle = FlatStyle.Flat, Width = 100, ForeColor = Color.White, BackColor = Color.FromArgb(20, 104, 163) };
        btnRef.FlatAppearance.BorderSize = 0;
        btnRef.Click += (_, _) => RefreshLogs();

        var btnExp = new Button { Text = "Export", FlatStyle = FlatStyle.Flat, Width = 100, ForeColor = Color.FromArgb(173, 220, 255), BackColor = Color.FromArgb(24, 36, 52) };
        btnExp.FlatAppearance.BorderColor = Color.FromArgb(43, 58, 76);
        btnExp.Click += (_, _) => ExportLogs();

        _cmbSeverity.SelectedIndexChanged += (_, _) => RefreshLogs();
        _txtSearch.TextChanged += (_, _) => RefreshLogs();

        _cmbSeverity.Location = new Point(10, 15);
        _txtSearch.Location = new Point(170, 15);
        btnRef.Location = new Point(430, 15);
        btnExp.Location = new Point(540, 15);
        pnlTop.Controls.AddRange([_cmbSeverity, _txtSearch, btnRef, btnExp]);

        Controls.Add(_gridLogs);
        Controls.Add(pnlTop);
        Controls.Add(new Label { Text = "LOG CENTER", Dock = DockStyle.Top, Font = new Font("Consolas", 14, FontStyle.Bold), Height = 40, ForeColor = Color.Cyan });
    }

    public void RefreshLogs()
    {
        var search = _txtSearch.Text.Trim();
        var severity = _cmbSeverity.SelectedItem?.ToString() ?? "All";
        var logs = _state.GetLogs().OrderByDescending(l => l.Timestamp).ToList();
        
        if (!string.IsNullOrWhiteSpace(search)) logs = logs.Where(l => l.Message.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        if (severity != "All" && Enum.TryParse<LogSeverity>(severity, out var sel)) logs = logs.Where(l => l.Severity == sel).ToList();

        var table = new DataTable();
        table.Columns.Add("Timestamp"); table.Columns.Add("Severity"); table.Columns.Add("Source"); table.Columns.Add("Message");
        foreach (var log in logs.Take(1000)) table.Rows.Add(log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), log.Severity.ToString(), log.Source, log.Message);
        
        _gridLogs.DataSource = table;
    }

    private void ExportLogs()
    {
        if (_gridLogs.DataSource is not DataTable table || table.Rows.Count == 0) return;
        using var fd = new SaveFileDialog { Filter = "TXT|*.txt", FileName = $"log-center-{DateTime.Now:yyyyMMdd-HHmmss}.txt" };
        if (fd.ShowDialog() == DialogResult.OK)
        {
            var sb = new StringBuilder();
            foreach (DataRow r in table.Rows) sb.AppendLine($"[{r[0]}] [{r[1]}] [{r[2]}] {r[3]}");
            File.WriteAllText(fd.FileName, sb.ToString());
            MessageBox.Show("Exportado");
        }
    }
}

public class SettingsUserControl : UserControl
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private ComboBox _cmbEnv = new() { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
    private ComboBox _cmbPreset = new() { Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
    public event EventHandler? OnSettingsApplied;

    public SettingsUserControl()
    {
        BackColor = Color.FromArgb(9, 9, 11);
        ForeColor = Color.Cyan;
        
        _cmbEnv.Items.AddRange(Enum.GetNames<DeploymentEnvironment>());
        _cmbEnv.SelectedItem = _state.CurrentEnvironment.ToString();
        _cmbPreset.Items.AddRange(_state.Presets.Select(p => p.Name).ToArray());

        var btnApply = new Button { Text = "Aplicar Preset y Entorno", FlatStyle = FlatStyle.Flat, Width = 250, Height = 40, ForeColor = Color.Cyan };
        btnApply.Click += (_, _) => {
            if (_cmbEnv.SelectedItem is string ev && Enum.TryParse<DeploymentEnvironment>(ev, out var ep)) _state.CurrentEnvironment = ep;
            OnSettingsApplied?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Configuración y Entorno aplicados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        var p = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 4, Padding = new Padding(20) };
        p.Controls.Add(new Label { Text = "Entorno Objetivo:", AutoSize=true, Anchor = AnchorStyles.Left, Font = new Font("Consolas", 11) }, 0, 0);
        p.Controls.Add(_cmbEnv, 1, 0);
        p.Controls.Add(new Label { Text = "Preset (Plantilla):", AutoSize=true, Anchor = AnchorStyles.Left, Font = new Font("Consolas", 11) }, 0, 1);
        p.Controls.Add(_cmbPreset, 1, 1);
        p.Controls.Add(btnApply, 1, 2);

        Controls.Add(p);
        Controls.Add(new Label { Text = "CONFIGURACIÓN Y PRESETS", Dock = DockStyle.Top, Font = new Font("Consolas", 14, FontStyle.Bold), Height = 40, ForeColor = Color.Cyan });
    }
}

public class AlertsUserControl : UserControl
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private DataGridView _grid = new();

    public AlertsUserControl()
    {
        BackColor = Color.FromArgb(9, 9, 11);
        _grid.Dock = DockStyle.Fill;
        UIHelper.AplicarEstiloModernoTabla(_grid);
        
        var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60 };
        var btnRef = new Button { Text = "Refrescar Alertas", FlatStyle = FlatStyle.Flat, Width = 150, ForeColor = Color.Cyan, Location = new Point(10, 15) };
        btnRef.Click += (_, _) => RefreshAlerts();
        pnlTop.Controls.Add(btnRef);

        Controls.Add(_grid);
        Controls.Add(pnlTop);
        Controls.Add(new Label { Text = "ALERTAS Y KPIs", Dock = DockStyle.Top, Font = new Font("Consolas", 14, FontStyle.Bold), Height = 40, ForeColor = Color.Cyan });
    }

    public void RefreshAlerts()
    {
        var table = new DataTable();
        table.Columns.Add("Fecha"); table.Columns.Add("Regla"); table.Columns.Add("Severidad"); table.Columns.Add("Mensaje");
        foreach (var a in _state.GetAlerts(false).OrderByDescending(x => x.CreatedAt))
            table.Rows.Add(a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), a.Rule, a.Severity, a.Message);
        
        _grid.DataSource = table;
    }
}