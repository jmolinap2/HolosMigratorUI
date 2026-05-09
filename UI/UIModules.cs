using System.Data;
using HolosMigratorUI.Core;
using System.Text;

namespace HolosMigratorUI.UI;

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
        btnApply.Click += (_, _) =>
        {
            if (_cmbEnv.SelectedItem is string ev && Enum.TryParse<DeploymentEnvironment>(ev, out var ep)) _state.CurrentEnvironment = ep;
            OnSettingsApplied?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Configuración y Entorno aplicados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        var p = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 4, Padding = new Padding(20) };
        p.Controls.Add(new Label { Text = "Entorno Objetivo:", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Consolas", 11) }, 0, 0);
        p.Controls.Add(_cmbEnv, 1, 0);
        p.Controls.Add(new Label { Text = "Preset (Plantilla):", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Consolas", 11) }, 0, 1);
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