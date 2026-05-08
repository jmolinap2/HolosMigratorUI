using HolosMigratorUI.Core;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace HolosMigratorUI;

public partial class ControlCenterForm : Form
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private readonly Func<string> _hostProvider;
    private readonly Func<int> _sshPortProvider;

    public ControlCenterForm(Func<string> hostProvider, Func<int> sshPortProvider)
    {
        _hostProvider = hostProvider;
        _sshPortProvider = sshPortProvider;
        InitializeComponent();
        BindEvents();
        LoadAll();
    }

    private void BindEvents()
    {
        _btnRefreshDashboard.Click += async (_, _) => await RefreshDashboardAsync();
        _btnRefreshLogs.Click += (_, _) => RefreshLogCenter();
        _btnExportFiltered.Click += (_, _) => ExportFiltered();
        _btnRefreshAlerts.Click += (_, _) => RefreshAlertsAndKpis();
        _btnApplyPreset.Click += (_, _) => ApplyPreset();
        _txtLogSearch.TextChanged += (_, _) => RefreshLogCenter();
        _cmbSeverityFilter.SelectedIndexChanged += (_, _) => RefreshLogCenter();
        _cmbEnvironment.SelectedIndexChanged += async (_, _) =>
        {
            if (_cmbEnvironment.SelectedItem is string env
                && Enum.TryParse<DeploymentEnvironment>(env, out var parsed))
            {
                _state.CurrentEnvironment = parsed;
                await RefreshDashboardAsync();
            }
        };
    }

    private void LoadAll()
    {
        _cmbSeverityFilter.Items.Clear();
        _cmbSeverityFilter.Items.AddRange(["All", "Info", "Warning", "Error", "Success"]);
        _cmbSeverityFilter.SelectedIndex = 0;

        _cmbEnvironment.Items.Clear();
        _cmbEnvironment.Items.AddRange(Enum.GetNames<DeploymentEnvironment>());
        _cmbEnvironment.SelectedItem = _state.CurrentEnvironment.ToString();

        _cmbPreset.Items.Clear();
        _cmbPreset.Items.AddRange(_state.Presets.Select(p => p.Name).ToArray());
        if (_cmbPreset.Items.Count > 0)
        {
            _cmbPreset.SelectedIndex = 0;
        }

        RefreshRuntimeLogs();
        RefreshLogCenter();
        RefreshAlertsAndKpis();
        _ = RefreshDashboardAsync();
    }

    private async Task RefreshDashboardAsync()
    {
        var host = _hostProvider();
        var port = _sshPortProvider();

        _lblHostValue.Text = host;
        _lblDashboardEnvironment.Text = _state.CurrentEnvironment.ToString().ToUpperInvariant();

        var networkReachable = await HealthCheckService.IsHostReachableAsync(host, port);
        var snapshot = _state.GetLatestHealthSnapshot(host) with { HostReachable = networkReachable };

        _lblHostStatus.Text = snapshot.HostReachable ? "Reachable" : "Unreachable";
        _lblSqlStatus.Text = snapshot.SqlRunning ? "Running" : "Not running";
        _lblApiStatus.Text = snapshot.ApiRunning ? "Running" : "Not running";
        _lblFrontStatus.Text = snapshot.FrontRunning ? "Running" : "Not running";
        _lblLastChecked.Text = snapshot.CheckedAt.ToString("yyyy-MM-dd HH:mm:ss");

        var runs = _state.GetRuns().OrderByDescending(r => r.EndedAt).Take(20).ToList();
        var table = new DataTable();
        table.Columns.Add("Inicio");
        table.Columns.Add("Fin");
        table.Columns.Add("Duración");
        table.Columns.Add("Acción");
        table.Columns.Add("Exit");
        table.Columns.Add("Resultado");

        foreach (var run in runs)
        {
            table.Rows.Add(
                run.StartedAt.ToString("MM-dd HH:mm:ss"),
                run.EndedAt.ToString("MM-dd HH:mm:ss"),
                $"{run.Duration.TotalMinutes:F1} min",
                run.Action,
                run.ExitCode.ToString(),
                run.Succeeded ? "OK" : "FAIL");
        }

        _gridRecentRuns.DataSource = table;
    }

    private void RefreshRuntimeLogs()
    {
        var lines = _state.GetLogs().OrderBy(l => l.Timestamp).TakeLast(1000)
            .Select(l => $"[{l.Timestamp:HH:mm:ss}] [{l.Severity}] {l.Message}");
        _txtRuntimeLog.Text = string.Join(Environment.NewLine, lines);
    }

    private void RefreshLogCenter()
    {
        var search = _txtLogSearch.Text.Trim();
        var severity = _cmbSeverityFilter.SelectedItem?.ToString() ?? "All";

        var logs = _state.GetLogs().OrderByDescending(l => l.Timestamp).ToList();
        if (!string.IsNullOrWhiteSpace(search))
        {
            logs = logs.Where(l => l.Message.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!severity.Equals("All", StringComparison.OrdinalIgnoreCase)
            && Enum.TryParse<LogSeverity>(severity, out var selectedSeverity))
        {
            logs = logs.Where(l => l.Severity == selectedSeverity).ToList();
        }

        var table = new DataTable();
        table.Columns.Add("Timestamp");
        table.Columns.Add("Severity");
        table.Columns.Add("Source");
        table.Columns.Add("Message");

        foreach (var log in logs.Take(2000))
        {
            table.Rows.Add(log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), log.Severity.ToString(), log.Source, log.Message);
        }

        _gridLogs.DataSource = table;

        RefreshRuntimeLogs();
    }

    private void ExportFiltered()
    {
        var table = _gridLogs.DataSource as DataTable;
        if (table == null || table.Rows.Count == 0)
        {
            MessageBox.Show("No hay filas para exportar.", "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "Archivo de texto (*.txt)|*.txt",
            FileName = $"log-center-{DateTime.Now:yyyyMMdd-HHmmss}.txt"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var sb = new StringBuilder();
        foreach (DataRow row in table.Rows)
        {
            sb.AppendLine($"[{row[0]}] [{row[1]}] [{row[2]}] {row[3]}");
        }

        File.WriteAllText(dialog.FileName, sb.ToString());
        MessageBox.Show("Exportación completada.", "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void RefreshAlertsAndKpis()
    {
        var alerts = _state.GetAlerts(onlyActive: false).OrderByDescending(a => a.CreatedAt).Take(200).ToList();
        var alertsTable = new DataTable();
        alertsTable.Columns.Add("Fecha");
        alertsTable.Columns.Add("Regla");
        alertsTable.Columns.Add("Severidad");
        alertsTable.Columns.Add("Mensaje");
        alertsTable.Columns.Add("Activa");

        foreach (var a in alerts)
        {
            alertsTable.Rows.Add(a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), a.Rule, a.Severity, a.Message, a.IsActive ? "Sí" : "No");
        }

        _gridAlerts.DataSource = alertsTable;

        var kpis = _state.GetKpis();
        _lblKpi7d.Text = $"{kpis.successRate7d:F2}%";
        _lblKpi30d.Text = $"{kpis.successRate30d:F2}%";
        _lblKpiMttr.Text = $"{kpis.mttrMinutes:F2} min";
        _lblKpiAvgDuration.Text = $"{kpis.avgDurationMinutes:F2} min";
    }

    private void ApplyPreset()
    {
        var selected = _cmbPreset.SelectedItem?.ToString();
        if (string.IsNullOrWhiteSpace(selected))
        {
            return;
        }

        var preset = _state.Presets.FirstOrDefault(p => p.Name == selected);
        if (preset == null)
        {
            return;
        }

        _txtPresetDetails.Text = $"Acción: {preset.Action}{Environment.NewLine}" +
                                 $"DeployTarget: {preset.DeployTarget}{Environment.NewLine}" +
                                 $"Migración: {preset.MigrationMode}{Environment.NewLine}" +
                                 $"SkipPull: {preset.SkipPull}{Environment.NewLine}" +
                                 $"SkipMigrations: {preset.SkipMigrations}{Environment.NewLine}" +
                                 $"SkipBuild: {preset.SkipBuild}{Environment.NewLine}" +
                                 $"SkipChecks: {preset.SkipPublicChecks}";

        if (_cmbEnvironment.SelectedItem is string env && Enum.TryParse<DeploymentEnvironment>(env, out var parsed))
        {
            _state.CurrentEnvironment = parsed;
        }

        MessageBox.Show("Preset aplicado al estado global. Puedes usarlo en la pantalla principal.", "Presets", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}