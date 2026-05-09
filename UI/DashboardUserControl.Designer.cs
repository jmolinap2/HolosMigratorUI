namespace HolosMigratorUI.UI;

partial class DashboardUserControl
{
    private System.ComponentModel.IContainer components = null!;

    private TableLayoutPanel _layoutRoot = null!;
    private TableLayoutPanel _layoutHeader = null!;
    private Panel _panelTitle = null!;
    private Label _lblSubtitleHeader = null!;
    private Label _lblTitleHeader = null!;
    private Button _btnRefresh = null!;

    private Panel _panelOverall = null!;
    private Label _lblOverallStatus = null!;
    private Label _lblLastChecked = null!;
    private Label _lblStateSource = null!;

    private TableLayoutPanel _layoutResources = null!;
    private Panel _panelCpuCard = null!;
    private Label _lblCpuTitle = null!;
    private Label _lblCpuUsage = null!;
    private Panel _panelMemoryCard = null!;
    private Label _lblMemoryTitle = null!;
    private Label _lblMemoryUsage = null!;
    private Panel _panelDiskCard = null!;
    private Label _lblDiskTitle = null!;
    private Label _lblDiskUsage = null!;
    private Panel _panelUptimeCard = null!;
    private Label _lblUptimeTitle = null!;
    private Label _lblUptime = null!;

    private TableLayoutPanel _layoutServices = null!;
    private Panel _panelHostCard = null!;
    private Label _lblHostTitle = null!;
    private Label _lblHostStatus = null!;
    private Panel _panelSqlCard = null!;
    private Label _lblSqlTitle = null!;
    private Label _lblSqlStatus = null!;
    private Panel _panelApiCard = null!;
    private Label _lblApiTitle = null!;
    private Label _lblApiStatus = null!;
    private Panel _panelFrontCard = null!;
    private Label _lblFrontTitle = null!;
    private Label _lblFrontStatus = null!;

    private TableLayoutPanel _layoutBottom = null!;
    private TableLayoutPanel _layoutKpis = null!;
    private Panel _panelKpi7d = null!;
    private Label _lblKpi7dTitle = null!;
    private Label _lblSuccess7d = null!;
    private Panel _panelKpi30d = null!;
    private Label _lblKpi30dTitle = null!;
    private Label _lblSuccess30d = null!;
    private Panel _panelKpiMttr = null!;
    private Label _lblKpiMttrTitle = null!;
    private Label _lblMttr = null!;
    private Panel _panelKpiAvg = null!;
    private Label _lblKpiAvgTitle = null!;
    private Label _lblAvgDuration = null!;

    private Panel _panelRuns = null!;
    private Label _lblRunsTitle = null!;
    private DataGridView _gridRecentRuns = null!;

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
        _layoutRoot = new TableLayoutPanel();
        _layoutHeader = new TableLayoutPanel();
        _panelTitle = new Panel();
        _lblTitleHeader = new Label();
        _lblSubtitleHeader = new Label();
        _btnRefresh = new Button();
        _panelOverall = new Panel();
        _lblStateSource = new Label();
        _lblLastChecked = new Label();
        _lblOverallStatus = new Label();
        _layoutResources = new TableLayoutPanel();
        _panelCpuCard = new Panel();
        _lblCpuUsage = new Label();
        _lblCpuTitle = new Label();
        _panelMemoryCard = new Panel();
        _lblMemoryUsage = new Label();
        _lblMemoryTitle = new Label();
        _panelDiskCard = new Panel();
        _lblDiskUsage = new Label();
        _lblDiskTitle = new Label();
        _panelUptimeCard = new Panel();
        _lblUptime = new Label();
        _lblUptimeTitle = new Label();
        _layoutServices = new TableLayoutPanel();
        _panelHostCard = new Panel();
        _lblHostStatus = new Label();
        _lblHostTitle = new Label();
        _panelSqlCard = new Panel();
        _lblSqlStatus = new Label();
        _lblSqlTitle = new Label();
        _panelApiCard = new Panel();
        _lblApiStatus = new Label();
        _lblApiTitle = new Label();
        _panelFrontCard = new Panel();
        _lblFrontStatus = new Label();
        _lblFrontTitle = new Label();
        _layoutBottom = new TableLayoutPanel();
        _layoutKpis = new TableLayoutPanel();
        _panelKpi7d = new Panel();
        _lblSuccess7d = new Label();
        _lblKpi7dTitle = new Label();
        _panelKpi30d = new Panel();
        _lblSuccess30d = new Label();
        _lblKpi30dTitle = new Label();
        _panelKpiMttr = new Panel();
        _lblMttr = new Label();
        _lblKpiMttrTitle = new Label();
        _panelKpiAvg = new Panel();
        _lblAvgDuration = new Label();
        _lblKpiAvgTitle = new Label();
        _panelRuns = new Panel();
        _gridRecentRuns = new DataGridView();
        _lblRunsTitle = new Label();
        _layoutRoot.SuspendLayout();
        _layoutHeader.SuspendLayout();
        _panelTitle.SuspendLayout();
        _panelOverall.SuspendLayout();
        _layoutResources.SuspendLayout();
        _panelCpuCard.SuspendLayout();
        _panelMemoryCard.SuspendLayout();
        _panelDiskCard.SuspendLayout();
        _panelUptimeCard.SuspendLayout();
        _layoutServices.SuspendLayout();
        _panelHostCard.SuspendLayout();
        _panelSqlCard.SuspendLayout();
        _panelApiCard.SuspendLayout();
        _panelFrontCard.SuspendLayout();
        _layoutBottom.SuspendLayout();
        _layoutKpis.SuspendLayout();
        _panelKpi7d.SuspendLayout();
        _panelKpi30d.SuspendLayout();
        _panelKpiMttr.SuspendLayout();
        _panelKpiAvg.SuspendLayout();
        _panelRuns.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_gridRecentRuns).BeginInit();
        SuspendLayout();
        // 
        // _layoutRoot
        // 
        _layoutRoot.ColumnCount = 1;
        _layoutRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _layoutRoot.Controls.Add(_layoutHeader, 0, 0);
        _layoutRoot.Controls.Add(_panelOverall, 0, 1);
        _layoutRoot.Controls.Add(_layoutResources, 0, 2);
        _layoutRoot.Controls.Add(_layoutServices, 0, 3);
        _layoutRoot.Controls.Add(_layoutBottom, 0, 4);
        _layoutRoot.Dock = DockStyle.Fill;
        _layoutRoot.Location = new Point(0, 0);
        _layoutRoot.Margin = new Padding(0);
        _layoutRoot.Name = "_layoutRoot";
        _layoutRoot.Padding = new Padding(28, 20, 28, 24);
        _layoutRoot.RowCount = 5;
        _layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
        _layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
        _layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 124F));
        _layoutRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 124F));
        _layoutRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _layoutRoot.Size = new Size(1500, 900);
        _layoutRoot.TabIndex = 0;
        // 
        // _layoutHeader
        // 
        _layoutHeader.ColumnCount = 2;
        _layoutHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _layoutHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
        _layoutHeader.Controls.Add(_panelTitle, 0, 0);
        _layoutHeader.Controls.Add(_btnRefresh, 1, 0);
        _layoutHeader.Dock = DockStyle.Fill;
        _layoutHeader.Location = new Point(22, 16);
        _layoutHeader.Margin = new Padding(0);
        _layoutHeader.Name = "_layoutHeader";
        _layoutHeader.RowCount = 1;
        _layoutHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _layoutHeader.Size = new Size(1456, 88);
        _layoutHeader.TabIndex = 0;
        // 
        // _panelTitle
        // 
        _panelTitle.BackColor = Color.Transparent;
        _panelTitle.Controls.Add(_lblTitleHeader);
        _panelTitle.Controls.Add(_lblSubtitleHeader);
        _panelTitle.Dock = DockStyle.Fill;
        _panelTitle.Location = new Point(3, 3);
        _panelTitle.Name = "_panelTitle";
        _panelTitle.Padding = new Padding(0, 12, 0, 0);
        _panelTitle.Size = new Size(1270, 82);
        _panelTitle.TabIndex = 0;
        // 
        // _lblTitleHeader
        // 
        _lblTitleHeader.Dock = DockStyle.Top;
        _lblTitleHeader.Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold);
        _lblTitleHeader.ForeColor = Color.FromArgb(244, 248, 252);
        _lblTitleHeader.Location = new Point(0, 34);
        _lblTitleHeader.Name = "_lblTitleHeader";
        _lblTitleHeader.Size = new Size(1270, 42);
        _lblTitleHeader.TabIndex = 0;
        _lblTitleHeader.Text = "CONTROL OPERATIVO DEL VPS";
        // 
        // _lblSubtitleHeader
        // 
        _lblSubtitleHeader.AutoEllipsis = true;
        _lblSubtitleHeader.Dock = DockStyle.Top;
        _lblSubtitleHeader.Font = new Font("Segoe UI", 10F);
        _lblSubtitleHeader.ForeColor = Color.FromArgb(186, 203, 222);
        _lblSubtitleHeader.Location = new Point(0, 8);
        _lblSubtitleHeader.Name = "_lblSubtitleHeader";
        _lblSubtitleHeader.Size = new Size(1270, 26);
        _lblSubtitleHeader.TabIndex = 1;
        _lblSubtitleHeader.Text = "Estado en tiempo real de conectividad y servicios principales";
        // 
        // _btnRefresh
        // 
        _btnRefresh.BackColor = Color.FromArgb(20, 104, 163);
        _btnRefresh.Dock = DockStyle.Fill;
        _btnRefresh.FlatAppearance.BorderSize = 0;
        _btnRefresh.FlatStyle = FlatStyle.Flat;
        _btnRefresh.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        _btnRefresh.ForeColor = Color.White;
        _btnRefresh.Location = new Point(1276, 10);
        _btnRefresh.Margin = new Padding(0, 10, 0, 10);
        _btnRefresh.Name = "_btnRefresh";
        _btnRefresh.Size = new Size(180, 68);
        _btnRefresh.TabIndex = 1;
        _btnRefresh.Text = "Actualizar";
        _btnRefresh.UseVisualStyleBackColor = false;
        // 
        // _panelOverall
        // 
        _panelOverall.BackColor = Color.FromArgb(16, 24, 34);
        _panelOverall.Controls.Add(_lblStateSource);
        _panelOverall.Controls.Add(_lblLastChecked);
        _panelOverall.Controls.Add(_lblOverallStatus);
        _panelOverall.Dock = DockStyle.Fill;
        _panelOverall.Location = new Point(22, 104);
        _panelOverall.Margin = new Padding(0);
        _panelOverall.Name = "_panelOverall";
        _panelOverall.Padding = new Padding(16, 10, 16, 8);
        _panelOverall.Size = new Size(1456, 90);
        _panelOverall.TabIndex = 1;
        // 
        // _lblStateSource
        // 
        _lblStateSource.AutoEllipsis = true;
        _lblStateSource.Dock = DockStyle.Top;
        _lblStateSource.Font = new Font("Segoe UI", 9.5F);
        _lblStateSource.ForeColor = Color.FromArgb(151, 169, 191);
        _lblStateSource.Location = new Point(16, 68);
        _lblStateSource.Name = "_lblStateSource";
        _lblStateSource.Size = new Size(1424, 22);
        _lblStateSource.TabIndex = 2;
        _lblStateSource.Text = "Fuente: --";
        // 
        // _lblLastChecked
        // 
        _lblLastChecked.Dock = DockStyle.Top;
        _lblLastChecked.Font = new Font("Segoe UI", 10.5F);
        _lblLastChecked.ForeColor = Color.FromArgb(187, 205, 226);
        _lblLastChecked.Location = new Point(16, 44);
        _lblLastChecked.Name = "_lblLastChecked";
        _lblLastChecked.Size = new Size(1424, 24);
        _lblLastChecked.TabIndex = 1;
        _lblLastChecked.Text = "Ultima verificacion: --";
        // 
        // _lblOverallStatus
        // 
        _lblOverallStatus.AutoEllipsis = true;
        _lblOverallStatus.Dock = DockStyle.Top;
        _lblOverallStatus.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
        _lblOverallStatus.ForeColor = Color.FromArgb(255, 209, 102);
        _lblOverallStatus.Location = new Point(16, 10);
        _lblOverallStatus.Name = "_lblOverallStatus";
        _lblOverallStatus.Size = new Size(1424, 34);
        _lblOverallStatus.TabIndex = 0;
        _lblOverallStatus.Text = "Estado VPS: Pendiente de verificacion";
        // 
        // _layoutResources
        // 
        _layoutResources.ColumnCount = 4;
        _layoutResources.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutResources.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutResources.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutResources.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutResources.Controls.Add(_panelCpuCard, 0, 0);
        _layoutResources.Controls.Add(_panelMemoryCard, 1, 0);
        _layoutResources.Controls.Add(_panelDiskCard, 2, 0);
        _layoutResources.Controls.Add(_panelUptimeCard, 3, 0);
        _layoutResources.Dock = DockStyle.Fill;
        _layoutResources.Location = new Point(22, 194);
        _layoutResources.Margin = new Padding(0);
        _layoutResources.Name = "_layoutResources";
        _layoutResources.Padding = new Padding(0, 12, 0, 12);
        _layoutResources.RowCount = 1;
        _layoutResources.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _layoutResources.Size = new Size(1456, 124);
        _layoutResources.TabIndex = 2;
        // 
        // _panelCpuCard
        // 
        _panelCpuCard.BackColor = Color.FromArgb(22, 30, 43);
        _panelCpuCard.Controls.Add(_lblCpuUsage);
        _panelCpuCard.Controls.Add(_lblCpuTitle);
        _panelCpuCard.Dock = DockStyle.Fill;
        _panelCpuCard.Location = new Point(6, 14);
        _panelCpuCard.Margin = new Padding(6);
        _panelCpuCard.Name = "_panelCpuCard";
        _panelCpuCard.Padding = new Padding(18, 14, 18, 14);
        _panelCpuCard.Size = new Size(352, 96);
        _panelCpuCard.TabIndex = 0;
        // 
        // _lblCpuUsage
        // 
        _lblCpuUsage.AutoEllipsis = true;
        _lblCpuUsage.Dock = DockStyle.Fill;
        _lblCpuUsage.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblCpuUsage.ForeColor = Color.FromArgb(120, 214, 255);
        _lblCpuUsage.Location = new Point(14, 36);
        _lblCpuUsage.Name = "_lblCpuUsage";
        _lblCpuUsage.Size = new Size(324, 50);
        _lblCpuUsage.TabIndex = 1;
        _lblCpuUsage.Text = "--";
        _lblCpuUsage.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblCpuTitle
        // 
        _lblCpuTitle.Dock = DockStyle.Top;
        _lblCpuTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblCpuTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblCpuTitle.Location = new Point(14, 10);
        _lblCpuTitle.Name = "_lblCpuTitle";
        _lblCpuTitle.Size = new Size(324, 26);
        _lblCpuTitle.TabIndex = 0;
        _lblCpuTitle.Text = "CPU VPS";
        // 
        // _panelMemoryCard
        // 
        _panelMemoryCard.BackColor = Color.FromArgb(22, 30, 43);
        _panelMemoryCard.Controls.Add(_lblMemoryUsage);
        _panelMemoryCard.Controls.Add(_lblMemoryTitle);
        _panelMemoryCard.Dock = DockStyle.Fill;
        _panelMemoryCard.Location = new Point(370, 14);
        _panelMemoryCard.Margin = new Padding(6);
        _panelMemoryCard.Name = "_panelMemoryCard";
        _panelMemoryCard.Padding = new Padding(18, 14, 18, 14);
        _panelMemoryCard.Size = new Size(352, 96);
        _panelMemoryCard.TabIndex = 1;
        // 
        // _lblMemoryUsage
        // 
        _lblMemoryUsage.AutoEllipsis = true;
        _lblMemoryUsage.Dock = DockStyle.Fill;
        _lblMemoryUsage.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblMemoryUsage.ForeColor = Color.FromArgb(188, 158, 255);
        _lblMemoryUsage.Location = new Point(14, 36);
        _lblMemoryUsage.Name = "_lblMemoryUsage";
        _lblMemoryUsage.Size = new Size(324, 50);
        _lblMemoryUsage.TabIndex = 1;
        _lblMemoryUsage.Text = "--";
        _lblMemoryUsage.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblMemoryTitle
        // 
        _lblMemoryTitle.Dock = DockStyle.Top;
        _lblMemoryTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblMemoryTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblMemoryTitle.Location = new Point(14, 10);
        _lblMemoryTitle.Name = "_lblMemoryTitle";
        _lblMemoryTitle.Size = new Size(324, 26);
        _lblMemoryTitle.TabIndex = 0;
        _lblMemoryTitle.Text = "MEMORIA VPS";
        // 
        // _panelDiskCard
        // 
        _panelDiskCard.BackColor = Color.FromArgb(22, 30, 43);
        _panelDiskCard.Controls.Add(_lblDiskUsage);
        _panelDiskCard.Controls.Add(_lblDiskTitle);
        _panelDiskCard.Dock = DockStyle.Fill;
        _panelDiskCard.Location = new Point(734, 14);
        _panelDiskCard.Margin = new Padding(6);
        _panelDiskCard.Name = "_panelDiskCard";
        _panelDiskCard.Padding = new Padding(18, 14, 18, 14);
        _panelDiskCard.Size = new Size(352, 96);
        _panelDiskCard.TabIndex = 2;
        // 
        // _lblDiskUsage
        // 
        _lblDiskUsage.AutoEllipsis = true;
        _lblDiskUsage.Dock = DockStyle.Fill;
        _lblDiskUsage.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblDiskUsage.ForeColor = Color.FromArgb(130, 198, 255);
        _lblDiskUsage.Location = new Point(14, 36);
        _lblDiskUsage.Name = "_lblDiskUsage";
        _lblDiskUsage.Size = new Size(324, 50);
        _lblDiskUsage.TabIndex = 1;
        _lblDiskUsage.Text = "--";
        _lblDiskUsage.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblDiskTitle
        // 
        _lblDiskTitle.Dock = DockStyle.Top;
        _lblDiskTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblDiskTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblDiskTitle.Location = new Point(14, 10);
        _lblDiskTitle.Name = "_lblDiskTitle";
        _lblDiskTitle.Size = new Size(324, 26);
        _lblDiskTitle.TabIndex = 0;
        _lblDiskTitle.Text = "DISCO /";
        // 
        // _panelUptimeCard
        // 
        _panelUptimeCard.BackColor = Color.FromArgb(22, 30, 43);
        _panelUptimeCard.Controls.Add(_lblUptime);
        _panelUptimeCard.Controls.Add(_lblUptimeTitle);
        _panelUptimeCard.Dock = DockStyle.Fill;
        _panelUptimeCard.Location = new Point(1098, 14);
        _panelUptimeCard.Margin = new Padding(6);
        _panelUptimeCard.Name = "_panelUptimeCard";
        _panelUptimeCard.Padding = new Padding(18, 14, 18, 14);
        _panelUptimeCard.Size = new Size(352, 96);
        _panelUptimeCard.TabIndex = 3;
        // 
        // _lblUptime
        // 
        _lblUptime.AutoEllipsis = true;
        _lblUptime.Dock = DockStyle.Fill;
        _lblUptime.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblUptime.ForeColor = Color.FromArgb(255, 209, 102);
        _lblUptime.Location = new Point(14, 36);
        _lblUptime.Name = "_lblUptime";
        _lblUptime.Size = new Size(324, 50);
        _lblUptime.TabIndex = 1;
        _lblUptime.Text = "--";
        _lblUptime.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblUptimeTitle
        // 
        _lblUptimeTitle.Dock = DockStyle.Top;
        _lblUptimeTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblUptimeTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblUptimeTitle.Location = new Point(14, 10);
        _lblUptimeTitle.Name = "_lblUptimeTitle";
        _lblUptimeTitle.Size = new Size(324, 26);
        _lblUptimeTitle.TabIndex = 0;
        _lblUptimeTitle.Text = "UPTIME";
        // 
        // _layoutServices
        // 
        _layoutServices.ColumnCount = 4;
        _layoutServices.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutServices.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutServices.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutServices.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutServices.Controls.Add(_panelHostCard, 0, 0);
        _layoutServices.Controls.Add(_panelSqlCard, 1, 0);
        _layoutServices.Controls.Add(_panelApiCard, 2, 0);
        _layoutServices.Controls.Add(_panelFrontCard, 3, 0);
        _layoutServices.Dock = DockStyle.Fill;
        _layoutServices.Location = new Point(22, 318);
        _layoutServices.Margin = new Padding(0);
        _layoutServices.Name = "_layoutServices";
        _layoutServices.Padding = new Padding(0, 12, 0, 12);
        _layoutServices.RowCount = 1;
        _layoutServices.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _layoutServices.Size = new Size(1456, 124);
        _layoutServices.TabIndex = 3;
        // 
        // _panelHostCard
        // 
        _panelHostCard.BackColor = Color.FromArgb(22, 30, 43);
        _panelHostCard.Controls.Add(_lblHostStatus);
        _panelHostCard.Controls.Add(_lblHostTitle);
        _panelHostCard.Dock = DockStyle.Fill;
        _panelHostCard.Location = new Point(6, 14);
        _panelHostCard.Margin = new Padding(6);
        _panelHostCard.Name = "_panelHostCard";
        _panelHostCard.Padding = new Padding(18, 14, 18, 14);
        _panelHostCard.Size = new Size(352, 96);
        _panelHostCard.TabIndex = 0;
        // 
        // _lblHostStatus
        // 
        _lblHostStatus.AutoEllipsis = true;
        _lblHostStatus.Dock = DockStyle.Fill;
        _lblHostStatus.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblHostStatus.ForeColor = Color.FromArgb(78, 214, 138);
        _lblHostStatus.Location = new Point(14, 36);
        _lblHostStatus.Name = "_lblHostStatus";
        _lblHostStatus.Size = new Size(324, 50);
        _lblHostStatus.TabIndex = 1;
        _lblHostStatus.Text = "--";
        _lblHostStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblHostTitle
        // 
        _lblHostTitle.Dock = DockStyle.Top;
        _lblHostTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblHostTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblHostTitle.Location = new Point(14, 10);
        _lblHostTitle.Name = "_lblHostTitle";
        _lblHostTitle.Size = new Size(324, 26);
        _lblHostTitle.TabIndex = 0;
        _lblHostTitle.Text = "HOST / SSH";
        // 
        // _panelSqlCard
        // 
        _panelSqlCard.BackColor = Color.FromArgb(22, 30, 43);
        _panelSqlCard.Controls.Add(_lblSqlStatus);
        _panelSqlCard.Controls.Add(_lblSqlTitle);
        _panelSqlCard.Dock = DockStyle.Fill;
        _panelSqlCard.Location = new Point(370, 14);
        _panelSqlCard.Margin = new Padding(6);
        _panelSqlCard.Name = "_panelSqlCard";
        _panelSqlCard.Padding = new Padding(18, 14, 18, 14);
        _panelSqlCard.Size = new Size(352, 96);
        _panelSqlCard.TabIndex = 1;
        // 
        // _lblSqlStatus
        // 
        _lblSqlStatus.AutoEllipsis = true;
        _lblSqlStatus.Dock = DockStyle.Fill;
        _lblSqlStatus.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblSqlStatus.ForeColor = Color.FromArgb(255, 129, 129);
        _lblSqlStatus.Location = new Point(14, 36);
        _lblSqlStatus.Name = "_lblSqlStatus";
        _lblSqlStatus.Size = new Size(324, 50);
        _lblSqlStatus.TabIndex = 1;
        _lblSqlStatus.Text = "--";
        _lblSqlStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblSqlTitle
        // 
        _lblSqlTitle.Dock = DockStyle.Top;
        _lblSqlTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblSqlTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblSqlTitle.Location = new Point(14, 10);
        _lblSqlTitle.Name = "_lblSqlTitle";
        _lblSqlTitle.Size = new Size(324, 26);
        _lblSqlTitle.TabIndex = 0;
        _lblSqlTitle.Text = "SQL";
        // 
        // _panelApiCard
        // 
        _panelApiCard.BackColor = Color.FromArgb(22, 30, 43);
        _panelApiCard.Controls.Add(_lblApiStatus);
        _panelApiCard.Controls.Add(_lblApiTitle);
        _panelApiCard.Dock = DockStyle.Fill;
        _panelApiCard.Location = new Point(734, 14);
        _panelApiCard.Margin = new Padding(6);
        _panelApiCard.Name = "_panelApiCard";
        _panelApiCard.Padding = new Padding(18, 14, 18, 14);
        _panelApiCard.Size = new Size(352, 96);
        _panelApiCard.TabIndex = 2;
        // 
        // _lblApiStatus
        // 
        _lblApiStatus.AutoEllipsis = true;
        _lblApiStatus.Dock = DockStyle.Fill;
        _lblApiStatus.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblApiStatus.ForeColor = Color.FromArgb(255, 129, 129);
        _lblApiStatus.Location = new Point(14, 36);
        _lblApiStatus.Name = "_lblApiStatus";
        _lblApiStatus.Size = new Size(324, 50);
        _lblApiStatus.TabIndex = 1;
        _lblApiStatus.Text = "--";
        _lblApiStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblApiTitle
        // 
        _lblApiTitle.Dock = DockStyle.Top;
        _lblApiTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblApiTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblApiTitle.Location = new Point(14, 10);
        _lblApiTitle.Name = "_lblApiTitle";
        _lblApiTitle.Size = new Size(324, 26);
        _lblApiTitle.TabIndex = 0;
        _lblApiTitle.Text = "API";
        // 
        // _panelFrontCard
        // 
        _panelFrontCard.BackColor = Color.FromArgb(22, 30, 43);
        _panelFrontCard.Controls.Add(_lblFrontStatus);
        _panelFrontCard.Controls.Add(_lblFrontTitle);
        _panelFrontCard.Dock = DockStyle.Fill;
        _panelFrontCard.Location = new Point(1098, 14);
        _panelFrontCard.Margin = new Padding(6);
        _panelFrontCard.Name = "_panelFrontCard";
        _panelFrontCard.Padding = new Padding(18, 14, 18, 14);
        _panelFrontCard.Size = new Size(352, 96);
        _panelFrontCard.TabIndex = 3;
        // 
        // _lblFrontStatus
        // 
        _lblFrontStatus.AutoEllipsis = true;
        _lblFrontStatus.Dock = DockStyle.Fill;
        _lblFrontStatus.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblFrontStatus.ForeColor = Color.FromArgb(255, 129, 129);
        _lblFrontStatus.Location = new Point(14, 36);
        _lblFrontStatus.Name = "_lblFrontStatus";
        _lblFrontStatus.Size = new Size(324, 50);
        _lblFrontStatus.TabIndex = 1;
        _lblFrontStatus.Text = "--";
        _lblFrontStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblFrontTitle
        // 
        _lblFrontTitle.Dock = DockStyle.Top;
        _lblFrontTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblFrontTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblFrontTitle.Location = new Point(14, 10);
        _lblFrontTitle.Name = "_lblFrontTitle";
        _lblFrontTitle.Size = new Size(324, 26);
        _lblFrontTitle.TabIndex = 0;
        _lblFrontTitle.Text = "FRONT";
        // 
        // _layoutBottom
        // 
        _layoutBottom.ColumnCount = 1;
        _layoutBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _layoutBottom.Controls.Add(_layoutKpis, 0, 0);
        _layoutBottom.Controls.Add(_panelRuns, 0, 1);
        _layoutBottom.Dock = DockStyle.Fill;
        _layoutBottom.Location = new Point(22, 442);
        _layoutBottom.Margin = new Padding(0);
        _layoutBottom.Name = "_layoutBottom";
        _layoutBottom.RowCount = 2;
        _layoutBottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
        _layoutBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _layoutBottom.Size = new Size(1456, 436);
        _layoutBottom.TabIndex = 4;
        // 
        // _layoutKpis
        // 
        _layoutKpis.ColumnCount = 4;
        _layoutKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        _layoutKpis.Controls.Add(_panelKpi7d, 0, 0);
        _layoutKpis.Controls.Add(_panelKpi30d, 1, 0);
        _layoutKpis.Controls.Add(_panelKpiMttr, 2, 0);
        _layoutKpis.Controls.Add(_panelKpiAvg, 3, 0);
        _layoutKpis.Dock = DockStyle.Fill;
        _layoutKpis.Location = new Point(0, 0);
        _layoutKpis.Margin = new Padding(0);
        _layoutKpis.Name = "_layoutKpis";
        _layoutKpis.Padding = new Padding(0, 12, 0, 12);
        _layoutKpis.RowCount = 1;
        _layoutKpis.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _layoutKpis.Size = new Size(1456, 112);
        _layoutKpis.TabIndex = 0;
        // 
        // _panelKpi7d
        // 
        _panelKpi7d.BackColor = Color.FromArgb(22, 30, 43);
        _panelKpi7d.Controls.Add(_lblSuccess7d);
        _panelKpi7d.Controls.Add(_lblKpi7dTitle);
        _panelKpi7d.Dock = DockStyle.Fill;
        _panelKpi7d.Location = new Point(6, 14);
        _panelKpi7d.Margin = new Padding(6);
        _panelKpi7d.Name = "_panelKpi7d";
        _panelKpi7d.Padding = new Padding(18, 14, 18, 14);
        _panelKpi7d.Size = new Size(352, 84);
        _panelKpi7d.TabIndex = 0;
        // 
        // _lblSuccess7d
        // 
        _lblSuccess7d.AutoEllipsis = true;
        _lblSuccess7d.Dock = DockStyle.Fill;
        _lblSuccess7d.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblSuccess7d.ForeColor = Color.FromArgb(229, 236, 244);
        _lblSuccess7d.Location = new Point(14, 36);
        _lblSuccess7d.Name = "_lblSuccess7d";
        _lblSuccess7d.Size = new Size(324, 38);
        _lblSuccess7d.TabIndex = 1;
        _lblSuccess7d.Text = "--";
        _lblSuccess7d.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblKpi7dTitle
        // 
        _lblKpi7dTitle.Dock = DockStyle.Top;
        _lblKpi7dTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblKpi7dTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblKpi7dTitle.Location = new Point(14, 10);
        _lblKpi7dTitle.Name = "_lblKpi7dTitle";
        _lblKpi7dTitle.Size = new Size(324, 26);
        _lblKpi7dTitle.TabIndex = 0;
        _lblKpi7dTitle.Text = "EXITO 7D";
        // 
        // _panelKpi30d
        // 
        _panelKpi30d.BackColor = Color.FromArgb(22, 30, 43);
        _panelKpi30d.Controls.Add(_lblSuccess30d);
        _panelKpi30d.Controls.Add(_lblKpi30dTitle);
        _panelKpi30d.Dock = DockStyle.Fill;
        _panelKpi30d.Location = new Point(370, 14);
        _panelKpi30d.Margin = new Padding(6);
        _panelKpi30d.Name = "_panelKpi30d";
        _panelKpi30d.Padding = new Padding(18, 14, 18, 14);
        _panelKpi30d.Size = new Size(352, 84);
        _panelKpi30d.TabIndex = 1;
        // 
        // _lblSuccess30d
        // 
        _lblSuccess30d.AutoEllipsis = true;
        _lblSuccess30d.Dock = DockStyle.Fill;
        _lblSuccess30d.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblSuccess30d.ForeColor = Color.FromArgb(229, 236, 244);
        _lblSuccess30d.Location = new Point(14, 36);
        _lblSuccess30d.Name = "_lblSuccess30d";
        _lblSuccess30d.Size = new Size(324, 38);
        _lblSuccess30d.TabIndex = 1;
        _lblSuccess30d.Text = "--";
        _lblSuccess30d.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblKpi30dTitle
        // 
        _lblKpi30dTitle.Dock = DockStyle.Top;
        _lblKpi30dTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblKpi30dTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblKpi30dTitle.Location = new Point(14, 10);
        _lblKpi30dTitle.Name = "_lblKpi30dTitle";
        _lblKpi30dTitle.Size = new Size(324, 26);
        _lblKpi30dTitle.TabIndex = 0;
        _lblKpi30dTitle.Text = "EXITO 30D";
        // 
        // _panelKpiMttr
        // 
        _panelKpiMttr.BackColor = Color.FromArgb(22, 30, 43);
        _panelKpiMttr.Controls.Add(_lblMttr);
        _panelKpiMttr.Controls.Add(_lblKpiMttrTitle);
        _panelKpiMttr.Dock = DockStyle.Fill;
        _panelKpiMttr.Location = new Point(734, 14);
        _panelKpiMttr.Margin = new Padding(6);
        _panelKpiMttr.Name = "_panelKpiMttr";
        _panelKpiMttr.Padding = new Padding(18, 14, 18, 14);
        _panelKpiMttr.Size = new Size(352, 84);
        _panelKpiMttr.TabIndex = 2;
        // 
        // _lblMttr
        // 
        _lblMttr.AutoEllipsis = true;
        _lblMttr.Dock = DockStyle.Fill;
        _lblMttr.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblMttr.ForeColor = Color.FromArgb(229, 236, 244);
        _lblMttr.Location = new Point(14, 36);
        _lblMttr.Name = "_lblMttr";
        _lblMttr.Size = new Size(324, 38);
        _lblMttr.TabIndex = 1;
        _lblMttr.Text = "--";
        _lblMttr.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblKpiMttrTitle
        // 
        _lblKpiMttrTitle.Dock = DockStyle.Top;
        _lblKpiMttrTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblKpiMttrTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblKpiMttrTitle.Location = new Point(14, 10);
        _lblKpiMttrTitle.Name = "_lblKpiMttrTitle";
        _lblKpiMttrTitle.Size = new Size(324, 26);
        _lblKpiMttrTitle.TabIndex = 0;
        _lblKpiMttrTitle.Text = "MTTR";
        // 
        // _panelKpiAvg
        // 
        _panelKpiAvg.BackColor = Color.FromArgb(22, 30, 43);
        _panelKpiAvg.Controls.Add(_lblAvgDuration);
        _panelKpiAvg.Controls.Add(_lblKpiAvgTitle);
        _panelKpiAvg.Dock = DockStyle.Fill;
        _panelKpiAvg.Location = new Point(1098, 14);
        _panelKpiAvg.Margin = new Padding(6);
        _panelKpiAvg.Name = "_panelKpiAvg";
        _panelKpiAvg.Padding = new Padding(18, 14, 18, 14);
        _panelKpiAvg.Size = new Size(352, 84);
        _panelKpiAvg.TabIndex = 3;
        // 
        // _lblAvgDuration
        // 
        _lblAvgDuration.AutoEllipsis = true;
        _lblAvgDuration.Dock = DockStyle.Fill;
        _lblAvgDuration.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        _lblAvgDuration.ForeColor = Color.FromArgb(229, 236, 244);
        _lblAvgDuration.Location = new Point(14, 36);
        _lblAvgDuration.Name = "_lblAvgDuration";
        _lblAvgDuration.Size = new Size(324, 38);
        _lblAvgDuration.TabIndex = 1;
        _lblAvgDuration.Text = "--";
        _lblAvgDuration.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _lblKpiAvgTitle
        // 
        _lblKpiAvgTitle.Dock = DockStyle.Top;
        _lblKpiAvgTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _lblKpiAvgTitle.ForeColor = Color.FromArgb(160, 183, 208);
        _lblKpiAvgTitle.Location = new Point(14, 10);
        _lblKpiAvgTitle.Name = "_lblKpiAvgTitle";
        _lblKpiAvgTitle.Size = new Size(324, 26);
        _lblKpiAvgTitle.TabIndex = 0;
        _lblKpiAvgTitle.Text = "DURACION PROM.";
        // 
        // _panelRuns
        // 
        _panelRuns.BackColor = Color.FromArgb(14, 20, 30);
        _panelRuns.Controls.Add(_gridRecentRuns);
        _panelRuns.Controls.Add(_lblRunsTitle);
        _panelRuns.Dock = DockStyle.Fill;
        _panelRuns.Location = new Point(0, 112);
        _panelRuns.Margin = new Padding(0);
        _panelRuns.Name = "_panelRuns";
        _panelRuns.Padding = new Padding(16);
        _panelRuns.Size = new Size(1456, 324);
        _panelRuns.TabIndex = 1;
        // 
        // _gridRecentRuns
        // 
        _gridRecentRuns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        _gridRecentRuns.Dock = DockStyle.Fill;
        _gridRecentRuns.Location = new Point(10, 40);
        _gridRecentRuns.Name = "_gridRecentRuns";
        _gridRecentRuns.RowHeadersWidth = 62;
        _gridRecentRuns.Size = new Size(1436, 274);
        _gridRecentRuns.TabIndex = 0;
        // 
        // _lblRunsTitle
        // 
        _lblRunsTitle.Dock = DockStyle.Top;
        _lblRunsTitle.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
        _lblRunsTitle.ForeColor = Color.FromArgb(170, 200, 235);
        _lblRunsTitle.Location = new Point(10, 10);
        _lblRunsTitle.Name = "_lblRunsTitle";
        _lblRunsTitle.Size = new Size(1436, 30);
        _lblRunsTitle.TabIndex = 1;
        _lblRunsTitle.Text = "EJECUCIONES RECIENTES";
        // 
        // DashboardUserControl
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(10, 14, 22);
        Controls.Add(_layoutRoot);
        ForeColor = Color.FromArgb(220, 230, 240);
        Name = "DashboardUserControl";
        Size = new Size(1500, 900);
        _layoutRoot.ResumeLayout(false);
        _layoutHeader.ResumeLayout(false);
        _panelTitle.ResumeLayout(false);
        _panelOverall.ResumeLayout(false);
        _layoutResources.ResumeLayout(false);
        _panelCpuCard.ResumeLayout(false);
        _panelMemoryCard.ResumeLayout(false);
        _panelDiskCard.ResumeLayout(false);
        _panelUptimeCard.ResumeLayout(false);
        _layoutServices.ResumeLayout(false);
        _panelHostCard.ResumeLayout(false);
        _panelSqlCard.ResumeLayout(false);
        _panelApiCard.ResumeLayout(false);
        _panelFrontCard.ResumeLayout(false);
        _layoutBottom.ResumeLayout(false);
        _layoutKpis.ResumeLayout(false);
        _panelKpi7d.ResumeLayout(false);
        _panelKpi30d.ResumeLayout(false);
        _panelKpiMttr.ResumeLayout(false);
        _panelKpiAvg.ResumeLayout(false);
        _panelRuns.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_gridRecentRuns).EndInit();
        ResumeLayout(false);
    }
}