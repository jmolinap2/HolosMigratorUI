namespace HolosMigratorUI;

partial class ControlCenterForm
{
    private System.ComponentModel.IContainer components = null;

    private TabControl _tabs;
    private TabPage _tabDashboard;
    private TabPage _tabLogCenter;
    private TabPage _tabSettings;
    private TabPage _tabAlerts;

    private Label _lblHost;
    private Label _lblHostValue;
    private Label _lblDashboardEnvironment;
    private Label _lblHostStatus;
    private Label _lblSqlStatus;
    private Label _lblApiStatus;
    private Label _lblFrontStatus;
    private Label _lblLastChecked;
    private Button _btnRefreshDashboard;
    private DataGridView _gridRecentRuns;

    private TextBox _txtRuntimeLog;
    private TextBox _txtLogSearch;
    private ComboBox _cmbSeverityFilter;
    private Button _btnRefreshLogs;
    private Button _btnExportFiltered;
    private DataGridView _gridLogs;

    private ComboBox _cmbEnvironment;
    private ComboBox _cmbPreset;
    private Button _btnApplyPreset;
    private TextBox _txtPresetDetails;

    private DataGridView _gridAlerts;
    private Button _btnRefreshAlerts;
    private Label _lblKpi7d;
    private Label _lblKpi30d;
    private Label _lblKpiMttr;
    private Label _lblKpiAvgDuration;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _tabs = new TabControl();
        _tabDashboard = new TabPage();
        _tabLogCenter = new TabPage();
        _tabSettings = new TabPage();
        _tabAlerts = new TabPage();

        _lblHost = new Label();
        _lblHostValue = new Label();
        _lblDashboardEnvironment = new Label();
        _lblHostStatus = new Label();
        _lblSqlStatus = new Label();
        _lblApiStatus = new Label();
        _lblFrontStatus = new Label();
        _lblLastChecked = new Label();
        _btnRefreshDashboard = new Button();
        _gridRecentRuns = new DataGridView();

        _txtRuntimeLog = new TextBox();
        _txtLogSearch = new TextBox();
        _cmbSeverityFilter = new ComboBox();
        _btnRefreshLogs = new Button();
        _btnExportFiltered = new Button();
        _gridLogs = new DataGridView();

        _cmbEnvironment = new ComboBox();
        _cmbPreset = new ComboBox();
        _btnApplyPreset = new Button();
        _txtPresetDetails = new TextBox();

        _gridAlerts = new DataGridView();
        _btnRefreshAlerts = new Button();
        _lblKpi7d = new Label();
        _lblKpi30d = new Label();
        _lblKpiMttr = new Label();
        _lblKpiAvgDuration = new Label();

        SuspendLayout();

        _tabs.Dock = DockStyle.Fill;
        _tabs.TabPages.AddRange([_tabDashboard, _tabLogCenter, _tabSettings, _tabAlerts]);

        _tabDashboard.Text = "Dashboard";
        _tabDashboard.BackColor = Color.FromArgb(12, 15, 22);
        _tabDashboard.ForeColor = Color.FromArgb(0, 255, 200);

        _lblHost.Text = "Host:";
        _lblHost.Location = new Point(14, 15);
        _lblHost.Size = new Size(60, 22);

        _lblHostValue.Text = "-";
        _lblHostValue.Location = new Point(80, 15);
        _lblHostValue.Size = new Size(260, 22);

        _lblDashboardEnvironment.Text = "ENV";
        _lblDashboardEnvironment.Location = new Point(360, 15);
        _lblDashboardEnvironment.Size = new Size(180, 22);

        _lblHostStatus.Text = "Host: -";
        _lblHostStatus.Location = new Point(14, 45);
        _lblHostStatus.Size = new Size(180, 22);

        _lblSqlStatus.Text = "SQL: -";
        _lblSqlStatus.Location = new Point(210, 45);
        _lblSqlStatus.Size = new Size(180, 22);

        _lblApiStatus.Text = "API: -";
        _lblApiStatus.Location = new Point(410, 45);
        _lblApiStatus.Size = new Size(180, 22);

        _lblFrontStatus.Text = "Front: -";
        _lblFrontStatus.Location = new Point(610, 45);
        _lblFrontStatus.Size = new Size(180, 22);

        _lblLastChecked.Text = "Checked: -";
        _lblLastChecked.Location = new Point(14, 72);
        _lblLastChecked.Size = new Size(260, 22);

        _btnRefreshDashboard.Text = "Refrescar";
        _btnRefreshDashboard.Location = new Point(820, 20);
        _btnRefreshDashboard.Size = new Size(120, 34);

        _gridRecentRuns.Location = new Point(14, 105);
        _gridRecentRuns.Size = new Size(930, 470);
        _gridRecentRuns.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _gridRecentRuns.ReadOnly = true;
        _gridRecentRuns.AllowUserToAddRows = false;
        _gridRecentRuns.AllowUserToDeleteRows = false;
        _gridRecentRuns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _tabDashboard.Controls.AddRange([
            _lblHost, _lblHostValue, _lblDashboardEnvironment,
            _lblHostStatus, _lblSqlStatus, _lblApiStatus, _lblFrontStatus,
            _lblLastChecked, _btnRefreshDashboard, _gridRecentRuns
        ]);

        _tabLogCenter.Text = "Log Center";
        _tabLogCenter.BackColor = Color.FromArgb(12, 15, 22);
        _tabLogCenter.ForeColor = Color.FromArgb(0, 255, 200);

        _txtRuntimeLog.Location = new Point(14, 14);
        _txtRuntimeLog.Size = new Size(930, 180);
        _txtRuntimeLog.Multiline = true;
        _txtRuntimeLog.ScrollBars = ScrollBars.Both;
        _txtRuntimeLog.ReadOnly = true;
        _txtRuntimeLog.Font = new Font("Consolas", 9F);
        _txtRuntimeLog.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        _txtLogSearch.Location = new Point(14, 208);
        _txtLogSearch.Size = new Size(300, 31);

        _cmbSeverityFilter.Location = new Point(330, 208);
        _cmbSeverityFilter.Size = new Size(150, 31);
        _cmbSeverityFilter.DropDownStyle = ComboBoxStyle.DropDownList;

        _btnRefreshLogs.Text = "Refrescar";
        _btnRefreshLogs.Location = new Point(500, 206);
        _btnRefreshLogs.Size = new Size(110, 34);

        _btnExportFiltered.Text = "Exportar";
        _btnExportFiltered.Location = new Point(620, 206);
        _btnExportFiltered.Size = new Size(110, 34);

        _gridLogs.Location = new Point(14, 250);
        _gridLogs.Size = new Size(930, 325);
        _gridLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _gridLogs.ReadOnly = true;
        _gridLogs.AllowUserToAddRows = false;
        _gridLogs.AllowUserToDeleteRows = false;
        _gridLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _tabLogCenter.Controls.AddRange([
            _txtRuntimeLog, _txtLogSearch, _cmbSeverityFilter, _btnRefreshLogs, _btnExportFiltered, _gridLogs
        ]);

        _tabSettings.Text = "Settings";
        _tabSettings.BackColor = Color.FromArgb(12, 15, 22);
        _tabSettings.ForeColor = Color.FromArgb(0, 255, 200);

        var lblEnv = new Label { Text = "Entorno:", Location = new Point(14, 20), Size = new Size(120, 24) };
        _cmbEnvironment.Location = new Point(140, 16);
        _cmbEnvironment.Size = new Size(220, 31);
        _cmbEnvironment.DropDownStyle = ComboBoxStyle.DropDownList;

        var lblPreset = new Label { Text = "Preset:", Location = new Point(14, 60), Size = new Size(120, 24) };
        _cmbPreset.Location = new Point(140, 56);
        _cmbPreset.Size = new Size(350, 31);
        _cmbPreset.DropDownStyle = ComboBoxStyle.DropDownList;

        _btnApplyPreset.Text = "Aplicar Preset";
        _btnApplyPreset.Location = new Point(510, 55);
        _btnApplyPreset.Size = new Size(140, 34);

        _txtPresetDetails.Location = new Point(14, 105);
        _txtPresetDetails.Size = new Size(930, 470);
        _txtPresetDetails.Multiline = true;
        _txtPresetDetails.ScrollBars = ScrollBars.Vertical;
        _txtPresetDetails.ReadOnly = true;
        _txtPresetDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _txtPresetDetails.Font = new Font("Consolas", 10F);

        _tabSettings.Controls.AddRange([lblEnv, _cmbEnvironment, lblPreset, _cmbPreset, _btnApplyPreset, _txtPresetDetails]);

        _tabAlerts.Text = "Alerts y KPI";
        _tabAlerts.BackColor = Color.FromArgb(12, 15, 22);
        _tabAlerts.ForeColor = Color.FromArgb(0, 255, 200);

        var lbl7d = new Label { Text = "Success 7d:", Location = new Point(14, 14), Size = new Size(120, 24) };
        _lblKpi7d.Location = new Point(140, 14);
        _lblKpi7d.Size = new Size(150, 24);

        var lbl30d = new Label { Text = "Success 30d:", Location = new Point(320, 14), Size = new Size(120, 24) };
        _lblKpi30d.Location = new Point(450, 14);
        _lblKpi30d.Size = new Size(150, 24);

        var lblMttr = new Label { Text = "MTTR:", Location = new Point(620, 14), Size = new Size(80, 24) };
        _lblKpiMttr.Location = new Point(700, 14);
        _lblKpiMttr.Size = new Size(120, 24);

        var lblAvg = new Label { Text = "Avg duración:", Location = new Point(14, 44), Size = new Size(120, 24) };
        _lblKpiAvgDuration.Location = new Point(140, 44);
        _lblKpiAvgDuration.Size = new Size(150, 24);

        _btnRefreshAlerts.Text = "Refrescar";
        _btnRefreshAlerts.Location = new Point(820, 22);
        _btnRefreshAlerts.Size = new Size(120, 34);

        _gridAlerts.Location = new Point(14, 80);
        _gridAlerts.Size = new Size(930, 495);
        _gridAlerts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _gridAlerts.ReadOnly = true;
        _gridAlerts.AllowUserToAddRows = false;
        _gridAlerts.AllowUserToDeleteRows = false;
        _gridAlerts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _tabAlerts.Controls.AddRange([
            lbl7d, _lblKpi7d, lbl30d, _lblKpi30d, lblMttr, _lblKpiMttr, lblAvg, _lblKpiAvgDuration,
            _btnRefreshAlerts, _gridAlerts
        ]);

        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(8, 10, 14);
        ClientSize = new Size(980, 640);
        Controls.Add(_tabs);
        Name = "ControlCenterForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Holos Migrator - Control Center";

        ResumeLayout(false);
    }
}