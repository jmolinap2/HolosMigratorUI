namespace HolosMigratorUI;

partial class MainShellForm
{
    private Panel _pnlSidebar;
    private Panel _pnlMainContent;
    private Panel _pnlOperations;

    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private TableLayoutPanel _tableRoot;
    private TableLayoutPanel _tableTop;
    private GroupBox _grpGeneral;
    private GroupBox _grpSsh;
    private GroupBox _grpOps;
    private GroupBox _grpLog;
    private FlowLayoutPanel _panelActions;
    private TableLayoutPanel _tblGeneral;
    private TableLayoutPanel _tblSsh;
    private TableLayoutPanel _tblOps;

    private Label _lblAction;
    private Label _lblRepoPath;
    private Label _lblRemoteRepoPath;
    private Label _lblBranch;
    private Label _lblComposeFile;
    private Label _lblServerHost;
    private Label _lblServerUser;
    private Label _lblSshPort;
    private Label _lblSshAuth;
    private Label _lblSshPassword;
    private Label _lblSshKeyPath;
    private Label _lblDeployTarget;
    private Label _lblMigrationMode;
    private Label _lblTenant;

    private ComboBox _cmbAction;
    private TextBox _txtRepoPath;
    private TextBox _txtRemoteRepoPath;
    private TextBox _txtBranch;
    private TextBox _txtComposeFile;
    private Label _lblGitToken;
    private TextBox _txtGitToken;

    private TextBox _txtServerHost;
    private TextBox _txtServerUser;
    private NumericUpDown _numSshPort;
    private ComboBox _cmbSshAuth;
    private Panel _panelSshPassword;
    private TextBox _txtSshPassword;
    private HolosMigratorUI.UI.AnimatedRoundedButton _btnShowPassword;
    private Panel _panelHeader;
    private Label _lblAppTitle;
    private Label _lblEnvironmentRisk;
    private FlowLayoutPanel _flpHeaderActions;

    private HolosMigratorUI.UI.AnimatedRoundedButton _btnControlCenter;
    private TextBox _txtSshKeyPath;
    private CheckBox _chkRememberSshPassword;
    private CheckBox _chkSshBatchMode;
    private CheckBox _chkInteractiveWindowForPassword;
    private CheckBox _chkSkipPull;
    private CheckBox _chkSkipMigrations;
    private CheckBox _chkSkipBuild;
    private CheckBox _chkSkipPublicChecks;

    private HolosMigratorUI.UI.AnimatedRoundedButton _btnRun;
    private HolosMigratorUI.UI.AnimatedRoundedButton _btnAdvanced;
    private HolosMigratorUI.UI.AnimatedRoundedButton _btnStop;
    private HolosMigratorUI.UI.AnimatedRoundedButton _btnOpenScripts;
    private HolosMigratorUI.UI.AnimatedRoundedButton _btnOpenLog;
    private TextBox _txtLog;
    private ProgressBar _progressBar;
    private Panel _panelStatus;
    private Label _lblStatus;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _pnlSidebar = new Panel();
        _pnlMainContent = new Panel();
        _pnlOperations = new Panel();
        _tableRoot = new TableLayoutPanel();
        _tableTop = new TableLayoutPanel();
        _grpGeneral = new GroupBox();
        _chkSkipMigrations = new CheckBox();
        _chkSkipPull = new CheckBox();
        _chkSkipBuild = new CheckBox();
        _chkSkipPublicChecks = new CheckBox();
        _tblGeneral = new TableLayoutPanel();
        _lblAction = new Label();
        _cmbAction = new ComboBox();
        _lblRepoPath = new Label();
        _txtRepoPath = new TextBox();
        _lblRemoteRepoPath = new Label();
        _txtRemoteRepoPath = new TextBox();
        _lblBranch = new Label();
        _txtBranch = new TextBox();
        _lblComposeFile = new Label();
        _txtComposeFile = new TextBox();
        _lblGitToken = new Label();
        _txtGitToken = new TextBox();
        _grpSsh = new GroupBox();
        _tblSsh = new TableLayoutPanel();
        _lblServerHost = new Label();
        _txtServerHost = new TextBox();
        _lblServerUser = new Label();
        _txtServerUser = new TextBox();
        _lblSshPort = new Label();
        _numSshPort = new NumericUpDown();
        _lblSshAuth = new Label();
        _cmbSshAuth = new ComboBox();
        _lblSshPassword = new Label();
        _panelSshPassword = new Panel();
        _txtSshPassword = new TextBox();
        _btnShowPassword = new HolosMigratorUI.UI.AnimatedRoundedButton();
        _chkRememberSshPassword = new CheckBox();
        _lblSshKeyPath = new Label();
        _txtSshKeyPath = new TextBox();
        _chkSshBatchMode = new CheckBox();
        _chkInteractiveWindowForPassword = new CheckBox();
        _grpOps = new GroupBox();
        _tblOps = new TableLayoutPanel();
        _lblDeployTarget = new Label();
        _cmbDeployTarget = new ComboBox();
        _lblMigrationMode = new Label();
        _cmbMigrationMode = new ComboBox();
        _lblTenant = new Label();
        _txtTenant = new TextBox();
        _panelSkipFlags = new FlowLayoutPanel();
        _grpLog = new GroupBox();
        _txtLog = new TextBox();
        _lblEnvironmentRisk = new Label();
        _progressBar = new ProgressBar();
        _panelStatus = new Panel();
        _lblStatus = new Label();
        _btnRun = new HolosMigratorUI.UI.AnimatedRoundedButton();
        _btnAdvanced = new HolosMigratorUI.UI.AnimatedRoundedButton();

        _btnControlCenter = new HolosMigratorUI.UI.AnimatedRoundedButton();
        _btnOpenScripts = new HolosMigratorUI.UI.AnimatedRoundedButton();
        _btnOpenLog = new HolosMigratorUI.UI.AnimatedRoundedButton();
        _btnStop = new HolosMigratorUI.UI.AnimatedRoundedButton();
        _panelActions = new FlowLayoutPanel();
        _panelHeader = new Panel();
        _flpHeaderActions = new FlowLayoutPanel();
        _lblAppTitle = new Label();
        _pnlMainContent.SuspendLayout();
        _pnlOperations.SuspendLayout();
        _tableRoot.SuspendLayout();
        _tableTop.SuspendLayout();
        _grpGeneral.SuspendLayout();
        _tblGeneral.SuspendLayout();
        _grpSsh.SuspendLayout();
        _tblSsh.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_numSshPort).BeginInit();
        _panelSshPassword.SuspendLayout();
        _grpOps.SuspendLayout();
        _tblOps.SuspendLayout();
        _grpLog.SuspendLayout();
        _panelStatus.SuspendLayout();
        _panelHeader.SuspendLayout();
        _flpHeaderActions.SuspendLayout();
        SuspendLayout();
        // 
        // _pnlSidebar
        // 
        _pnlSidebar.BackColor = Color.FromArgb(15, 17, 26);
        _pnlSidebar.Dock = DockStyle.Left;
        _pnlSidebar.Location = new Point(0, 74);
        _pnlSidebar.Name = "_pnlSidebar";
        _pnlSidebar.Size = new Size(250, 976);
        _pnlSidebar.TabIndex = 2;
        // 
        // _pnlMainContent
        // 
        _pnlMainContent.BackColor = Color.FromArgb(9, 9, 11);
        _pnlMainContent.Controls.Add(_pnlOperations);
        _pnlMainContent.Dock = DockStyle.Fill;
        _pnlMainContent.Location = new Point(250, 74);
        _pnlMainContent.Name = "_pnlMainContent";
        _pnlMainContent.Size = new Size(1579, 976);
        _pnlMainContent.TabIndex = 3;
        // 
        // _pnlOperations
        // 
        _pnlOperations.Controls.Add(_tableRoot);
        _pnlOperations.Dock = DockStyle.Fill;
        _pnlOperations.Location = new Point(0, 0);
        _pnlOperations.Name = "_pnlOperations";
        _pnlOperations.Size = new Size(1579, 976);
        _pnlOperations.TabIndex = 4;
        // 
        // _tableRoot
        // 
        _tableRoot.BackColor = Color.FromArgb(9, 9, 11);
        _tableRoot.ColumnCount = 1;
        _tableRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tableRoot.Controls.Add(_tableTop, 0, 0);
        _tableRoot.Controls.Add(_grpLog, 0, 1);
        _tableRoot.Controls.Add(_progressBar, 0, 2);
        _tableRoot.Controls.Add(_panelStatus, 0, 3);
        _tableRoot.Dock = DockStyle.Fill;
        _tableRoot.Location = new Point(0, 0);
        _tableRoot.Margin = new Padding(4, 5, 4, 5);
        _tableRoot.Name = "_tableRoot";
        _tableRoot.Padding = new Padding(20, 16, 20, 16);
        _tableRoot.RowCount = 4;
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        _tableRoot.Size = new Size(1579, 976);
        _tableRoot.TabIndex = 0;
        // 
        // _tableTop
        // 
        _tableTop.ColumnCount = 3;
        _tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        _tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
        _tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        _tableTop.Controls.Add(_grpGeneral, 0, 0);
        _tableTop.Controls.Add(_grpSsh, 1, 0);
        _tableTop.Controls.Add(_grpOps, 2, 0);
        _tableTop.Dock = DockStyle.Fill;
        _tableTop.Location = new Point(24, 21);
        _tableTop.Margin = new Padding(4, 5, 4, 5);
        _tableTop.Name = "_tableTop";
        _tableTop.RowCount = 1;
        _tableTop.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tableTop.Size = new Size(1531, 388);
        _tableTop.TabIndex = 0;
        // 
        // _grpGeneral
        // 
        _grpGeneral.BackColor = Color.FromArgb(18, 18, 23);
        _grpGeneral.Controls.Add(_chkSkipMigrations);
        _grpGeneral.Controls.Add(_chkSkipPull);
        _grpGeneral.Controls.Add(_chkSkipBuild);
        _grpGeneral.Controls.Add(_chkSkipPublicChecks);
        _grpGeneral.Controls.Add(_tblGeneral);
        _grpGeneral.Dock = DockStyle.Fill;
        _grpGeneral.FlatStyle = FlatStyle.Flat;
        _grpGeneral.Font = new Font("Consolas", 11F, FontStyle.Bold);
        _grpGeneral.ForeColor = Color.FromArgb(0, 255, 255);
        _grpGeneral.Location = new Point(9, 10);
        _grpGeneral.Margin = new Padding(9, 10, 9, 10);
        _grpGeneral.Name = "_grpGeneral";
        _grpGeneral.Padding = new Padding(15);
        _grpGeneral.Size = new Size(502, 368);
        _grpGeneral.TabIndex = 0;
        _grpGeneral.TabStop = false;
        _grpGeneral.Text = "General";
        // 
        // _chkSkipMigrations
        // 
        _chkSkipMigrations.AutoSize = true;
        _chkSkipMigrations.ForeColor = Color.FromArgb(0, 255, 65);
        _chkSkipMigrations.Location = new Point(282, 406);
        _chkSkipMigrations.Margin = new Padding(0, 4, 16, 4);
        _chkSkipMigrations.Name = "_chkSkipMigrations";
        _chkSkipMigrations.Size = new Size(254, 30);
        _chkSkipMigrations.TabIndex = 1;
        _chkSkipMigrations.Text = "Omitir migraciones";
        // 
        // _chkSkipPull
        // 
        _chkSkipPull.AutoSize = true;
        _chkSkipPull.ForeColor = Color.FromArgb(0, 255, 65);
        _chkSkipPull.Location = new Point(28, 459);
        _chkSkipPull.Margin = new Padding(0, 4, 16, 4);
        _chkSkipPull.Name = "_chkSkipPull";
        _chkSkipPull.Size = new Size(170, 30);
        _chkSkipPull.TabIndex = 0;
        _chkSkipPull.Text = "Omitir pull";
        // 
        // _chkSkipBuild
        // 
        _chkSkipBuild.AutoSize = true;
        _chkSkipBuild.ForeColor = Color.FromArgb(0, 255, 65);
        _chkSkipBuild.Location = new Point(282, 459);
        _chkSkipBuild.Margin = new Padding(0, 4, 16, 4);
        _chkSkipBuild.Name = "_chkSkipBuild";
        _chkSkipBuild.Size = new Size(182, 30);
        _chkSkipBuild.TabIndex = 2;
        _chkSkipBuild.Text = "Omitir build";
        // 
        // _chkSkipPublicChecks
        // 
        _chkSkipPublicChecks.AutoSize = true;
        _chkSkipPublicChecks.ForeColor = Color.FromArgb(0, 255, 65);
        _chkSkipPublicChecks.Location = new Point(28, 406);
        _chkSkipPublicChecks.Margin = new Padding(0, 4, 0, 4);
        _chkSkipPublicChecks.Name = "_chkSkipPublicChecks";
        _chkSkipPublicChecks.Size = new Size(302, 30);
        _chkSkipPublicChecks.TabIndex = 3;
        _chkSkipPublicChecks.Text = "Omitir checks públicos";
        // 
        // _tblGeneral
        // 
        _tblGeneral.AutoSize = true;
        _tblGeneral.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _tblGeneral.BackColor = Color.FromArgb(18, 18, 23);
        _tblGeneral.ColumnCount = 2;
        _tblGeneral.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 243F));
        _tblGeneral.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tblGeneral.Controls.Add(_lblAction, 0, 0);
        _tblGeneral.Controls.Add(_cmbAction, 1, 0);
        _tblGeneral.Controls.Add(_lblRepoPath, 0, 1);
        _tblGeneral.Controls.Add(_txtRepoPath, 1, 1);
        _tblGeneral.Controls.Add(_lblRemoteRepoPath, 0, 2);
        _tblGeneral.Controls.Add(_txtRemoteRepoPath, 1, 2);
        _tblGeneral.Controls.Add(_lblBranch, 0, 3);
        _tblGeneral.Controls.Add(_txtBranch, 1, 3);
        _tblGeneral.Controls.Add(_lblComposeFile, 0, 4);
        _tblGeneral.Controls.Add(_txtComposeFile, 1, 4);
        _tblGeneral.Controls.Add(_lblGitToken, 0, 5);
        _tblGeneral.Controls.Add(_txtGitToken, 1, 5);
        _tblGeneral.Dock = DockStyle.Top;
        _tblGeneral.Font = new Font("Consolas", 10F);
        _tblGeneral.ForeColor = Color.FromArgb(0, 255, 65);
        _tblGeneral.Location = new Point(15, 41);
        _tblGeneral.Margin = new Padding(4, 5, 4, 5);
        _tblGeneral.Name = "_tblGeneral";
        _tblGeneral.RowCount = 6;
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.Size = new Size(472, 234);
        _tblGeneral.TabIndex = 0;
        // 
        // _lblAction
        // 
        _lblAction.Anchor = AnchorStyles.Left;
        _lblAction.AutoSize = true;
        _lblAction.Location = new Point(4, 8);
        _lblAction.Margin = new Padding(4, 6, 4, 6);
        _lblAction.Name = "_lblAction";
        _lblAction.Size = new Size(87, 23);
        _lblAction.TabIndex = 0;
        _lblAction.Text = "Acción:";
        // 
        // _cmbAction
        // 
        _cmbAction.BackColor = Color.FromArgb(9, 9, 11);
        _cmbAction.Dock = DockStyle.Fill;
        _cmbAction.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbAction.FlatStyle = FlatStyle.Flat;
        _cmbAction.ForeColor = Color.FromArgb(0, 255, 255);
        _cmbAction.Items.AddRange(new object[] { "Deploy completo", "Solo migraciones" });
        _cmbAction.Location = new Point(247, 4);
        _cmbAction.Margin = new Padding(4);
        _cmbAction.Name = "_cmbAction";
        _cmbAction.Size = new Size(221, 31);
        _cmbAction.TabIndex = 1;
        // 
        // _lblRepoPath
        // 
        _lblRepoPath.Anchor = AnchorStyles.Left;
        _lblRepoPath.AutoSize = true;
        _lblRepoPath.Location = new Point(4, 47);
        _lblRepoPath.Margin = new Padding(4, 6, 4, 6);
        _lblRepoPath.Name = "_lblRepoPath";
        _lblRepoPath.Size = new Size(131, 23);
        _lblRepoPath.TabIndex = 2;
        _lblRepoPath.Text = "Repo local:";
        // 
        // _txtRepoPath
        // 
        _txtRepoPath.BackColor = Color.FromArgb(9, 9, 11);
        _txtRepoPath.BorderStyle = BorderStyle.FixedSingle;
        _txtRepoPath.Dock = DockStyle.Fill;
        _txtRepoPath.ForeColor = Color.FromArgb(0, 255, 255);
        _txtRepoPath.Location = new Point(247, 43);
        _txtRepoPath.Margin = new Padding(4);
        _txtRepoPath.Name = "_txtRepoPath";
        _txtRepoPath.Size = new Size(221, 31);
        _txtRepoPath.TabIndex = 3;
        _txtRepoPath.Text = "C:\\Repos\\OmniSuite";
        // 
        // _lblRemoteRepoPath
        // 
        _lblRemoteRepoPath.Anchor = AnchorStyles.Left;
        _lblRemoteRepoPath.AutoSize = true;
        _lblRemoteRepoPath.Location = new Point(4, 86);
        _lblRemoteRepoPath.Margin = new Padding(4, 6, 4, 6);
        _lblRemoteRepoPath.Name = "_lblRemoteRepoPath";
        _lblRemoteRepoPath.Size = new Size(142, 23);
        _lblRemoteRepoPath.TabIndex = 4;
        _lblRemoteRepoPath.Text = "Repo remoto:";
        // 
        // _txtRemoteRepoPath
        // 
        _txtRemoteRepoPath.BackColor = Color.FromArgb(9, 9, 11);
        _txtRemoteRepoPath.BorderStyle = BorderStyle.FixedSingle;
        _txtRemoteRepoPath.Dock = DockStyle.Fill;
        _txtRemoteRepoPath.ForeColor = Color.FromArgb(0, 255, 255);
        _txtRemoteRepoPath.Location = new Point(247, 82);
        _txtRemoteRepoPath.Margin = new Padding(4);
        _txtRemoteRepoPath.Name = "_txtRemoteRepoPath";
        _txtRemoteRepoPath.Size = new Size(221, 31);
        _txtRemoteRepoPath.TabIndex = 5;
        _txtRemoteRepoPath.Text = "/root/OmniSuite";
        // 
        // _lblBranch
        // 
        _lblBranch.Anchor = AnchorStyles.Left;
        _lblBranch.AutoSize = true;
        _lblBranch.Location = new Point(4, 125);
        _lblBranch.Margin = new Padding(4, 6, 4, 6);
        _lblBranch.Name = "_lblBranch";
        _lblBranch.Size = new Size(65, 23);
        _lblBranch.TabIndex = 6;
        _lblBranch.Text = "Rama:";
        // 
        // _txtBranch
        // 
        _txtBranch.BackColor = Color.FromArgb(9, 9, 11);
        _txtBranch.BorderStyle = BorderStyle.FixedSingle;
        _txtBranch.Dock = DockStyle.Fill;
        _txtBranch.ForeColor = Color.FromArgb(0, 255, 255);
        _txtBranch.Location = new Point(247, 121);
        _txtBranch.Margin = new Padding(4);
        _txtBranch.Name = "_txtBranch";
        _txtBranch.Size = new Size(221, 31);
        _txtBranch.TabIndex = 7;
        _txtBranch.Text = "develop";
        // 
        // _lblComposeFile
        // 
        _lblComposeFile.Anchor = AnchorStyles.Left;
        _lblComposeFile.AutoSize = true;
        _lblComposeFile.Location = new Point(4, 164);
        _lblComposeFile.Margin = new Padding(4, 6, 4, 6);
        _lblComposeFile.Name = "_lblComposeFile";
        _lblComposeFile.Size = new Size(186, 23);
        _lblComposeFile.TabIndex = 8;
        _lblComposeFile.Text = "Archivo compose:";
        // 
        // _txtComposeFile
        // 
        _txtComposeFile.BackColor = Color.FromArgb(9, 9, 11);
        _txtComposeFile.BorderStyle = BorderStyle.FixedSingle;
        _txtComposeFile.Dock = DockStyle.Fill;
        _txtComposeFile.ForeColor = Color.FromArgb(0, 255, 255);
        _txtComposeFile.Location = new Point(247, 160);
        _txtComposeFile.Margin = new Padding(4);
        _txtComposeFile.Name = "_txtComposeFile";
        _txtComposeFile.Size = new Size(221, 31);
        _txtComposeFile.TabIndex = 9;
        _txtComposeFile.Text = "docker-compose.hostinger.yml";
        // 
        // _lblGitToken
        // 
        _lblGitToken.Anchor = AnchorStyles.Left;
        _lblGitToken.AutoSize = true;
        _lblGitToken.Location = new Point(4, 203);
        _lblGitToken.Margin = new Padding(4, 6, 4, 6);
        _lblGitToken.Name = "_lblGitToken";
        _lblGitToken.Size = new Size(153, 23);
        _lblGitToken.TabIndex = 10;
        _lblGitToken.Text = "Token GitHub:";
        // 
        // _txtGitToken
        // 
        _txtGitToken.BackColor = Color.FromArgb(9, 9, 11);
        _txtGitToken.BorderStyle = BorderStyle.FixedSingle;
        _txtGitToken.Dock = DockStyle.Fill;
        _txtGitToken.ForeColor = Color.FromArgb(0, 255, 255);
        _txtGitToken.Location = new Point(247, 199);
        _txtGitToken.Margin = new Padding(4);
        _txtGitToken.Name = "_txtGitToken";
        _txtGitToken.Size = new Size(221, 31);
        _txtGitToken.TabIndex = 11;
        _txtGitToken.UseSystemPasswordChar = true;
        // 
        // _grpSsh
        // 
        _grpSsh.BackColor = Color.FromArgb(18, 18, 23);
        _grpSsh.Controls.Add(_tblSsh);
        _grpSsh.Dock = DockStyle.Fill;
        _grpSsh.FlatStyle = FlatStyle.Flat;
        _grpSsh.Font = new Font("Consolas", 11F, FontStyle.Bold);
        _grpSsh.ForeColor = Color.FromArgb(0, 255, 255);
        _grpSsh.Location = new Point(529, 10);
        _grpSsh.Margin = new Padding(9, 10, 9, 10);
        _grpSsh.Name = "_grpSsh";
        _grpSsh.Padding = new Padding(10, 12, 10, 10);
        _grpSsh.Size = new Size(533, 368);
        _grpSsh.TabIndex = 1;
        _grpSsh.TabStop = false;
        _grpSsh.Text = "SSH";
        // 
        // _tblSsh
        // 
        _tblSsh.AutoSize = true;
        _tblSsh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _tblSsh.BackColor = Color.FromArgb(18, 18, 23);
        _tblSsh.ColumnCount = 2;
        _tblSsh.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 243F));
        _tblSsh.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tblSsh.Controls.Add(_lblServerHost, 0, 0);
        _tblSsh.Controls.Add(_txtServerHost, 1, 0);
        _tblSsh.Controls.Add(_lblServerUser, 0, 1);
        _tblSsh.Controls.Add(_txtServerUser, 1, 1);
        _tblSsh.Controls.Add(_lblSshPort, 0, 2);
        _tblSsh.Controls.Add(_numSshPort, 1, 2);
        _tblSsh.Controls.Add(_lblSshAuth, 0, 3);
        _tblSsh.Controls.Add(_cmbSshAuth, 1, 3);
        _tblSsh.Controls.Add(_lblSshPassword, 0, 4);
        _tblSsh.Controls.Add(_panelSshPassword, 1, 4);
        _tblSsh.Controls.Add(_chkRememberSshPassword, 1, 5);
        _tblSsh.Controls.Add(_lblSshKeyPath, 0, 6);
        _tblSsh.Controls.Add(_txtSshKeyPath, 1, 6);
        _tblSsh.Controls.Add(_chkSshBatchMode, 1, 7);
        _tblSsh.Controls.Add(_chkInteractiveWindowForPassword, 1, 8);
        _tblSsh.Dock = DockStyle.Top;
        _tblSsh.Font = new Font("Consolas", 10F);
        _tblSsh.ForeColor = Color.FromArgb(0, 255, 65);
        _tblSsh.Location = new Point(10, 38);
        _tblSsh.Margin = new Padding(4, 5, 4, 5);
        _tblSsh.Name = "_tblSsh";
        _tblSsh.RowCount = 9;
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.RowStyles.Add(new RowStyle());
        _tblSsh.Size = new Size(513, 363);
        _tblSsh.TabIndex = 0;
        // 
        // _lblServerHost
        // 
        _lblServerHost.Anchor = AnchorStyles.Left;
        _lblServerHost.AutoSize = true;
        _lblServerHost.Location = new Point(4, 11);
        _lblServerHost.Margin = new Padding(4, 6, 4, 6);
        _lblServerHost.Name = "_lblServerHost";
        _lblServerHost.Size = new Size(109, 23);
        _lblServerHost.TabIndex = 0;
        _lblServerHost.Text = "Servidor:";
        // 
        // _txtServerHost
        // 
        _txtServerHost.BackColor = Color.FromArgb(9, 9, 11);
        _txtServerHost.BorderStyle = BorderStyle.FixedSingle;
        _txtServerHost.Dock = DockStyle.Fill;
        _txtServerHost.ForeColor = Color.FromArgb(0, 255, 255);
        _txtServerHost.Location = new Point(247, 7);
        _txtServerHost.Margin = new Padding(4, 7, 4, 7);
        _txtServerHost.Name = "_txtServerHost";
        _txtServerHost.Size = new Size(262, 31);
        _txtServerHost.TabIndex = 1;
        // 
        // _lblServerUser
        // 
        _lblServerUser.Anchor = AnchorStyles.Left;
        _lblServerUser.AutoSize = true;
        _lblServerUser.Location = new Point(4, 56);
        _lblServerUser.Margin = new Padding(4, 6, 4, 6);
        _lblServerUser.Name = "_lblServerUser";
        _lblServerUser.Size = new Size(98, 23);
        _lblServerUser.TabIndex = 2;
        _lblServerUser.Text = "Usuario:";
        // 
        // _txtServerUser
        // 
        _txtServerUser.BackColor = Color.FromArgb(9, 9, 11);
        _txtServerUser.BorderStyle = BorderStyle.FixedSingle;
        _txtServerUser.Dock = DockStyle.Fill;
        _txtServerUser.ForeColor = Color.FromArgb(0, 255, 255);
        _txtServerUser.Location = new Point(247, 52);
        _txtServerUser.Margin = new Padding(4, 7, 4, 7);
        _txtServerUser.Name = "_txtServerUser";
        _txtServerUser.Size = new Size(262, 31);
        _txtServerUser.TabIndex = 3;
        // 
        // _lblSshPort
        // 
        _lblSshPort.Anchor = AnchorStyles.Left;
        _lblSshPort.AutoSize = true;
        _lblSshPort.Location = new Point(4, 101);
        _lblSshPort.Margin = new Padding(4, 6, 4, 6);
        _lblSshPort.Name = "_lblSshPort";
        _lblSshPort.Size = new Size(131, 23);
        _lblSshPort.TabIndex = 4;
        _lblSshPort.Text = "Puerto SSH:";
        // 
        // _numSshPort
        // 
        _numSshPort.BackColor = Color.FromArgb(9, 9, 11);
        _numSshPort.BorderStyle = BorderStyle.FixedSingle;
        _numSshPort.Dock = DockStyle.Fill;
        _numSshPort.ForeColor = Color.FromArgb(0, 255, 255);
        _numSshPort.Location = new Point(247, 97);
        _numSshPort.Margin = new Padding(4, 7, 4, 7);
        _numSshPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        _numSshPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        _numSshPort.Name = "_numSshPort";
        _numSshPort.Size = new Size(262, 31);
        _numSshPort.TabIndex = 5;
        _numSshPort.Value = new decimal(new int[] { 22, 0, 0, 0 });
        // 
        // _lblSshAuth
        // 
        _lblSshAuth.Anchor = AnchorStyles.Left;
        _lblSshAuth.AutoSize = true;
        _lblSshAuth.Location = new Point(4, 143);
        _lblSshAuth.Margin = new Padding(4, 6, 4, 6);
        _lblSshAuth.Name = "_lblSshAuth";
        _lblSshAuth.Size = new Size(109, 23);
        _lblSshAuth.TabIndex = 6;
        _lblSshAuth.Text = "Modo SSH:";
        // 
        // _cmbSshAuth
        // 
        _cmbSshAuth.BackColor = Color.FromArgb(9, 9, 11);
        _cmbSshAuth.Dock = DockStyle.Fill;
        _cmbSshAuth.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbSshAuth.FlatStyle = FlatStyle.Flat;
        _cmbSshAuth.ForeColor = Color.FromArgb(0, 255, 255);
        _cmbSshAuth.Items.AddRange(new object[] { "Auto - Llave si existe, si no password", "Key - Solo llave SSH", "Password - Pedir clave SSH" });
        _cmbSshAuth.Location = new Point(247, 139);
        _cmbSshAuth.Margin = new Padding(4);
        _cmbSshAuth.Name = "_cmbSshAuth";
        _cmbSshAuth.Size = new Size(262, 31);
        _cmbSshAuth.TabIndex = 7;
        // 
        // _lblSshPassword
        // 
        _lblSshPassword.Anchor = AnchorStyles.Left;
        _lblSshPassword.AutoSize = true;
        _lblSshPassword.Location = new Point(4, 185);
        _lblSshPassword.Margin = new Padding(4, 6, 4, 6);
        _lblSshPassword.Name = "_lblSshPassword";
        _lblSshPassword.Size = new Size(153, 23);
        _lblSshPassword.TabIndex = 8;
        _lblSshPassword.Text = "Password/Passphrase:";
        // 
        // _panelSshPassword
        // 
        _panelSshPassword.BackColor = Color.FromArgb(9, 9, 11);
        _panelSshPassword.BorderStyle = BorderStyle.FixedSingle;
        _panelSshPassword.Controls.Add(_txtSshPassword);
        _panelSshPassword.Controls.Add(_btnShowPassword);
        _panelSshPassword.Dock = DockStyle.Fill;
        _panelSshPassword.Location = new Point(247, 178);
        _panelSshPassword.Margin = new Padding(4);
        _panelSshPassword.Name = "_panelSshPassword";
        _panelSshPassword.Size = new Size(262, 37);
        _panelSshPassword.TabIndex = 9;
        // 
        // _txtSshPassword
        // 
        _txtSshPassword.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _txtSshPassword.BackColor = Color.FromArgb(9, 9, 11);
        _txtSshPassword.BorderStyle = BorderStyle.None;
        _txtSshPassword.ForeColor = Color.FromArgb(0, 255, 255);
        _txtSshPassword.Location = new Point(3, 3);
        _txtSshPassword.Name = "_txtSshPassword";
        _txtSshPassword.Size = new Size(220, 24);
        _txtSshPassword.TabIndex = 0;
        _txtSshPassword.UseSystemPasswordChar = true;
        // 
        // _btnShowPassword
        // 
        _btnShowPassword.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        _btnShowPassword.BackColor = Color.FromArgb(9, 9, 11);
        _btnShowPassword.Cursor = Cursors.Hand;
        _btnShowPassword.FlatAppearance.BorderSize = 0;
        _btnShowPassword.FlatStyle = FlatStyle.Flat;
        _btnShowPassword.ForeColor = Color.FromArgb(0, 255, 255);
        _btnShowPassword.Location = new Point(226, 0);
        _btnShowPassword.Name = "_btnShowPassword";
        _btnShowPassword.Size = new Size(36, 0);
        _btnShowPassword.TabIndex = 1;
        _btnShowPassword.TabStop = false;
        _btnShowPassword.Text = "👁";
        _btnShowPassword.UseVisualStyleBackColor = false;
        // 
        // _chkRememberSshPassword
        // 
        _chkRememberSshPassword.AutoSize = true;
        _chkRememberSshPassword.ForeColor = Color.FromArgb(0, 255, 65);
        _chkRememberSshPassword.Location = new Point(247, 223);
        _chkRememberSshPassword.Margin = new Padding(4);
        _chkRememberSshPassword.Name = "_chkRememberSshPassword";
        _chkRememberSshPassword.Size = new Size(262, 27);
        _chkRememberSshPassword.TabIndex = 10;
        _chkRememberSshPassword.Text = "Recordar clave SSH (cifrada)";
        // 
        // _lblSshKeyPath
        // 
        _lblSshKeyPath.Anchor = AnchorStyles.Left;
        _lblSshKeyPath.AutoSize = true;
        _lblSshKeyPath.Location = new Point(4, 262);
        _lblSshKeyPath.Margin = new Padding(4, 6, 4, 6);
        _lblSshKeyPath.Name = "_lblSshKeyPath";
        _lblSshKeyPath.Size = new Size(131, 23);
        _lblSshKeyPath.TabIndex = 11;
        _lblSshKeyPath.Text = "Ruta llave:";
        // 
        // _txtSshKeyPath
        // 
        _txtSshKeyPath.BackColor = Color.FromArgb(9, 9, 11);
        _txtSshKeyPath.BorderStyle = BorderStyle.FixedSingle;
        _txtSshKeyPath.Dock = DockStyle.Fill;
        _txtSshKeyPath.ForeColor = Color.FromArgb(0, 255, 255);
        _txtSshKeyPath.Location = new Point(247, 258);
        _txtSshKeyPath.Margin = new Padding(4);
        _txtSshKeyPath.Name = "_txtSshKeyPath";
        _txtSshKeyPath.Size = new Size(262, 31);
        _txtSshKeyPath.TabIndex = 12;
        _txtSshKeyPath.Text = "C:\\Users\\USER\\.ssh\\id_ed25519";
        // 
        // _chkSshBatchMode
        // 
        _chkSshBatchMode.AutoSize = true;
        _chkSshBatchMode.ForeColor = Color.FromArgb(0, 255, 65);
        _chkSshBatchMode.Location = new Point(247, 297);
        _chkSshBatchMode.Margin = new Padding(4);
        _chkSshBatchMode.Name = "_chkSshBatchMode";
        _chkSshBatchMode.Size = new Size(262, 27);
        _chkSshBatchMode.TabIndex = 13;
        _chkSshBatchMode.Text = "SshBatchMode (sin prompts)";
        // 
        // _chkInteractiveWindowForPassword
        // 
        _chkInteractiveWindowForPassword.AutoSize = true;
        _chkInteractiveWindowForPassword.Checked = true;
        _chkInteractiveWindowForPassword.CheckState = CheckState.Checked;
        _chkInteractiveWindowForPassword.ForeColor = Color.FromArgb(0, 255, 65);
        _chkInteractiveWindowForPassword.Location = new Point(247, 332);
        _chkInteractiveWindowForPassword.Margin = new Padding(4);
        _chkInteractiveWindowForPassword.Name = "_chkInteractiveWindowForPassword";
        _chkInteractiveWindowForPassword.Size = new Size(262, 27);
        _chkInteractiveWindowForPassword.TabIndex = 14;
        _chkInteractiveWindowForPassword.Text = "Abrir terminal interactiva para Password";
        // 
        // _grpOps
        // 
        _grpOps.BackColor = Color.FromArgb(18, 18, 23);
        _grpOps.Controls.Add(_tblOps);
        _grpOps.Dock = DockStyle.Fill;
        _grpOps.FlatStyle = FlatStyle.Flat;
        _grpOps.Font = new Font("Consolas", 11F, FontStyle.Bold);
        _grpOps.ForeColor = Color.FromArgb(0, 255, 255);
        _grpOps.Location = new Point(1080, 10);
        _grpOps.Margin = new Padding(9, 10, 9, 10);
        _grpOps.Name = "_grpOps";
        _grpOps.Padding = new Padding(10, 12, 10, 10);
        _grpOps.Size = new Size(442, 368);
        _grpOps.TabIndex = 1;
        _grpOps.TabStop = false;
        _grpOps.Text = "Opciones de despliegue y migración";
        // 
        // _tblOps
        // 
        _tblOps.AutoSize = true;
        _tblOps.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _tblOps.BackColor = Color.FromArgb(18, 18, 23);
        _tblOps.ColumnCount = 2;
        _tblOps.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        _tblOps.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tblOps.Controls.Add(_lblDeployTarget, 0, 0);
        _tblOps.Controls.Add(_cmbDeployTarget, 1, 0);
        _tblOps.Controls.Add(_lblMigrationMode, 0, 1);
        _tblOps.Controls.Add(_cmbMigrationMode, 1, 1);
        _tblOps.Controls.Add(_lblTenant, 0, 2);
        _tblOps.Controls.Add(_txtTenant, 1, 2);
        _tblOps.Controls.Add(_panelSkipFlags, 1, 3);
        _tblOps.Dock = DockStyle.Top;
        _tblOps.Font = new Font("Consolas", 10F);
        _tblOps.ForeColor = Color.FromArgb(0, 255, 65);
        _tblOps.Location = new Point(10, 38);
        _tblOps.Margin = new Padding(4, 5, 4, 5);
        _tblOps.Name = "_tblOps";
        _tblOps.RowCount = 4;
        _tblOps.RowStyles.Add(new RowStyle());
        _tblOps.RowStyles.Add(new RowStyle());
        _tblOps.RowStyles.Add(new RowStyle());
        _tblOps.RowStyles.Add(new RowStyle());
        _tblOps.Size = new Size(422, 182);
        _tblOps.TabIndex = 0;
        // 
        // _lblDeployTarget
        // 
        _lblDeployTarget.Anchor = AnchorStyles.Left;
        _lblDeployTarget.AutoSize = true;
        _lblDeployTarget.Location = new Point(4, 6);
        _lblDeployTarget.Margin = new Padding(4, 6, 4, 6);
        _lblDeployTarget.Name = "_lblDeployTarget";
        _lblDeployTarget.Size = new Size(109, 46);
        _lblDeployTarget.TabIndex = 0;
        _lblDeployTarget.Text = "Objetivo deploy:";
        // 
        // _cmbDeployTarget
        // 
        _cmbDeployTarget.BackColor = Color.FromArgb(9, 9, 11);
        _cmbDeployTarget.Dock = DockStyle.Fill;
        _cmbDeployTarget.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbDeployTarget.FlatStyle = FlatStyle.Flat;
        _cmbDeployTarget.ForeColor = Color.FromArgb(0, 255, 255);
        _cmbDeployTarget.Items.AddRange(new object[] { "Both - Front + API", "Backend - Solo API", "Frontend - Solo Front" });
        _cmbDeployTarget.Location = new Point(144, 4);
        _cmbDeployTarget.Margin = new Padding(4);
        _cmbDeployTarget.Name = "_cmbDeployTarget";
        _cmbDeployTarget.Size = new Size(274, 31);
        _cmbDeployTarget.TabIndex = 1;
        // 
        // _lblMigrationMode
        // 
        _lblMigrationMode.Anchor = AnchorStyles.Left;
        _lblMigrationMode.AutoSize = true;
        _lblMigrationMode.Location = new Point(4, 64);
        _lblMigrationMode.Margin = new Padding(4, 6, 4, 6);
        _lblMigrationMode.Name = "_lblMigrationMode";
        _lblMigrationMode.Size = new Size(120, 46);
        _lblMigrationMode.TabIndex = 2;
        _lblMigrationMode.Text = "Modo migración:";
        // 
        // _cmbMigrationMode
        // 
        _cmbMigrationMode.BackColor = Color.FromArgb(9, 9, 11);
        _cmbMigrationMode.Dock = DockStyle.Fill;
        _cmbMigrationMode.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbMigrationMode.FlatStyle = FlatStyle.Flat;
        _cmbMigrationMode.ForeColor = Color.FromArgb(0, 255, 255);
        _cmbMigrationMode.Items.AddRange(new object[] { "B - Host + todos los tenants", "H - Solo Host", "T - Solo todos los tenants", "U - Un tenant especifico", "N - Ninguna" });
        _cmbMigrationMode.Location = new Point(144, 62);
        _cmbMigrationMode.Margin = new Padding(4);
        _cmbMigrationMode.Name = "_cmbMigrationMode";
        _cmbMigrationMode.Size = new Size(274, 31);
        _cmbMigrationMode.TabIndex = 3;
        // 
        // _lblTenant
        // 
        _lblTenant.Anchor = AnchorStyles.Left;
        _lblTenant.AutoSize = true;
        _lblTenant.Location = new Point(4, 122);
        _lblTenant.Margin = new Padding(4, 6, 4, 6);
        _lblTenant.Name = "_lblTenant";
        _lblTenant.Size = new Size(109, 46);
        _lblTenant.TabIndex = 4;
        _lblTenant.Text = "Tenant (solo U):";
        // 
        // _txtTenant
        // 
        _txtTenant.BackColor = Color.FromArgb(9, 9, 11);
        _txtTenant.BorderStyle = BorderStyle.FixedSingle;
        _txtTenant.Dock = DockStyle.Fill;
        _txtTenant.ForeColor = Color.FromArgb(0, 255, 255);
        _txtTenant.Location = new Point(144, 120);
        _txtTenant.Margin = new Padding(4);
        _txtTenant.Name = "_txtTenant";
        _txtTenant.Size = new Size(274, 31);
        _txtTenant.TabIndex = 5;
        // 
        // _panelSkipFlags
        // 
        _panelSkipFlags.AutoSize = true;
        _panelSkipFlags.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _panelSkipFlags.Dock = DockStyle.Fill;
        _panelSkipFlags.Location = new Point(140, 178);
        _panelSkipFlags.Margin = new Padding(0, 4, 0, 4);
        _panelSkipFlags.Name = "_panelSkipFlags";
        _panelSkipFlags.Size = new Size(282, 1);
        _panelSkipFlags.TabIndex = 6;
        // 
        // _grpLog
        // 
        _grpLog.BackColor = Color.FromArgb(18, 18, 23);
        _grpLog.Controls.Add(_txtLog);
        _grpLog.Controls.Add(_lblEnvironmentRisk);
        _grpLog.Dock = DockStyle.Fill;
        _grpLog.ForeColor = Color.FromArgb(0, 255, 255);
        _grpLog.Location = new Point(29, 424);
        _grpLog.Margin = new Padding(9, 10, 9, 10);
        _grpLog.Name = "_grpLog";
        _grpLog.Padding = new Padding(10, 12, 10, 10);
        _grpLog.Size = new Size(1521, 467);
        _grpLog.TabIndex = 3;
        _grpLog.TabStop = false;
        _grpLog.Text = "Salida en tiempo real";
        // 
        // _txtLog
        // 
        _txtLog.BackColor = Color.FromArgb(22, 27, 34);
        _txtLog.Dock = DockStyle.Fill;
        _txtLog.Font = new Font("Consolas", 10F);
        _txtLog.ForeColor = Color.FromArgb(0, 255, 65);
        _txtLog.Location = new Point(10, 36);
        _txtLog.Margin = new Padding(4, 5, 4, 5);
        _txtLog.Multiline = true;
        _txtLog.Name = "_txtLog";
        _txtLog.ReadOnly = true;
        _txtLog.ScrollBars = ScrollBars.Both;
        _txtLog.Size = new Size(1501, 421);
        _txtLog.TabIndex = 0;
        // 
        // _lblEnvironmentRisk
        // 
        _lblEnvironmentRisk.BackColor = Color.FromArgb(5, 5, 5);
        _lblEnvironmentRisk.Font = new Font("Consolas", 10F, FontStyle.Bold);
        _lblEnvironmentRisk.ForeColor = Color.FromArgb(255, 209, 102);
        _lblEnvironmentRisk.Location = new Point(190, 13);
        _lblEnvironmentRisk.Name = "_lblEnvironmentRisk";
        _lblEnvironmentRisk.Size = new Size(350, 23);
        _lblEnvironmentRisk.TabIndex = 3;
        _lblEnvironmentRisk.Text = "VPS: SIN VERIFICAR";
        _lblEnvironmentRisk.TextAlign = ContentAlignment.MiddleRight;
        // 
        // _progressBar
        // 
        _progressBar.BackColor = Color.FromArgb(18, 18, 23);
        _progressBar.Dock = DockStyle.Fill;
        _progressBar.ForeColor = Color.FromArgb(0, 255, 255);
        _progressBar.Location = new Point(40, 901);
        _progressBar.Margin = new Padding(20, 0, 20, 0);
        _progressBar.MarqueeAnimationSpeed = 30;
        _progressBar.Name = "_progressBar";
        _progressBar.Size = new Size(1499, 18);
        _progressBar.Style = ProgressBarStyle.Marquee;
        _progressBar.TabIndex = 4;
        _progressBar.Visible = false;
        // 
        // _panelStatus
        // 
        _panelStatus.BackColor = Color.FromArgb(18, 18, 23);
        _panelStatus.Controls.Add(_lblStatus);
        _panelStatus.Dock = DockStyle.Fill;
        _panelStatus.Location = new Point(40, 923);
        _panelStatus.Margin = new Padding(20, 4, 20, 4);
        _panelStatus.Name = "_panelStatus";
        _panelStatus.Size = new Size(1499, 33);
        _panelStatus.TabIndex = 5;
        _panelStatus.Visible = false;
        // 
        // _lblStatus
        // 
        _lblStatus.Dock = DockStyle.Fill;
        _lblStatus.Font = new Font("Consolas", 11F, FontStyle.Bold);
        _lblStatus.ForeColor = Color.FromArgb(0, 255, 65);
        _lblStatus.Location = new Point(0, 0);
        _lblStatus.Name = "_lblStatus";
        _lblStatus.Size = new Size(1499, 33);
        _lblStatus.TabIndex = 0;
        _lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _btnRun
        // 
        _btnRun.BackColor = Color.FromArgb(0, 40, 0);
        _btnRun.Cursor = Cursors.Hand;
        _btnRun.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 65);
        _btnRun.FlatStyle = FlatStyle.Flat;
        _btnRun.Font = new Font("Consolas", 10F, FontStyle.Bold);
        _btnRun.ForeColor = Color.FromArgb(0, 255, 65);
        _btnRun.Location = new Point(850, 24);
        _btnRun.Margin = new Padding(0, 10, 6, 10);
        _btnRun.Name = "_btnRun";
        _btnRun.Size = new Size(140, 44);
        _btnRun.TabIndex = 0;
        _btnRun.Text = "EJECUTAR";
        _btnRun.UseVisualStyleBackColor = false;
        // 
        // _btnAdvanced
        // 
        _btnAdvanced.BackColor = Color.FromArgb(30, 58, 70);
        _btnAdvanced.Cursor = Cursors.Hand;
        _btnAdvanced.FlatAppearance.BorderColor = Color.FromArgb(100, 220, 255);
        _btnAdvanced.FlatStyle = FlatStyle.Flat;
        _btnAdvanced.Font = new Font("Consolas", 10F, FontStyle.Bold);
        _btnAdvanced.ForeColor = Color.FromArgb(100, 220, 255);
        _btnAdvanced.Location = new Point(0, 24);
        _btnAdvanced.Margin = new Padding(0, 10, 10, 10);
        _btnAdvanced.Name = "_btnAdvanced";
        _btnAdvanced.Size = new Size(180, 44);
        _btnAdvanced.TabIndex = 4;
        _btnAdvanced.Text = "[ SYS.ADVANCED ]";
        _btnAdvanced.UseVisualStyleBackColor = false;
        // 

        // 













        // 
        // _btnControlCenter
        // 
        _btnControlCenter.BackColor = Color.FromArgb(18, 18, 23);
        _btnControlCenter.Cursor = Cursors.Hand;
        _btnControlCenter.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255);
        _btnControlCenter.FlatStyle = FlatStyle.Flat;
        _btnControlCenter.Font = new Font("Consolas", 10F, FontStyle.Bold);
        _btnControlCenter.ForeColor = Color.FromArgb(0, 255, 255);
        _btnControlCenter.Location = new Point(390, 24);
        _btnControlCenter.Margin = new Padding(0, 10, 10, 10);
        _btnControlCenter.Name = "_btnControlCenter";
        _btnControlCenter.Size = new Size(160, 44);
        _btnControlCenter.TabIndex = 6;
        _btnControlCenter.Text = "OPERACIONES";
        _btnControlCenter.UseVisualStyleBackColor = false;
        // 
        // _btnOpenScripts
        // 
        _btnOpenScripts.BackColor = Color.FromArgb(18, 18, 23);
        _btnOpenScripts.Cursor = Cursors.Hand;
        _btnOpenScripts.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255);
        _btnOpenScripts.FlatStyle = FlatStyle.Flat;
        _btnOpenScripts.Font = new Font("Consolas", 10F, FontStyle.Bold);
        _btnOpenScripts.ForeColor = Color.FromArgb(0, 255, 255);
        _btnOpenScripts.Location = new Point(700, 24);
        _btnOpenScripts.Margin = new Padding(0, 10, 10, 10);
        _btnOpenScripts.Name = "_btnOpenScripts";
        _btnOpenScripts.Size = new Size(140, 44);
        _btnOpenScripts.TabIndex = 2;
        _btnOpenScripts.Text = "SCRIPTS";
        _btnOpenScripts.UseVisualStyleBackColor = false;
        // 
        // _btnOpenLog
        // 
        _btnOpenLog.BackColor = Color.FromArgb(30, 58, 70);
        _btnOpenLog.Cursor = Cursors.Hand;
        _btnOpenLog.FlatAppearance.BorderColor = Color.FromArgb(100, 220, 255);
        _btnOpenLog.FlatStyle = FlatStyle.Flat;
        _btnOpenLog.Font = new Font("Consolas", 10F, FontStyle.Bold);
        _btnOpenLog.ForeColor = Color.FromArgb(100, 220, 255);
        _btnOpenLog.Location = new Point(560, 24);
        _btnOpenLog.Margin = new Padding(0, 10, 10, 10);
        _btnOpenLog.Name = "_btnOpenLog";
        _btnOpenLog.Size = new Size(130, 44);
        _btnOpenLog.TabIndex = 3;
        _btnOpenLog.Text = "LOG LOCAL";
        _btnOpenLog.UseVisualStyleBackColor = false;
        // 
        // _btnStop
        // 
        _btnStop.BackColor = Color.FromArgb(60, 10, 10);
        _btnStop.Cursor = Cursors.Hand;
        _btnStop.Enabled = true;
        _btnStop.FlatAppearance.BorderColor = Color.FromArgb(255, 60, 60);
        _btnStop.FlatStyle = FlatStyle.Flat;
        _btnStop.Font = new Font("Consolas", 10F, FontStyle.Bold);
        _btnStop.ForeColor = Color.FromArgb(255, 220, 220);
        _btnStop.Location = new Point(996, 24);
        _btnStop.Margin = new Padding(0, 10, 6, 10);
        _btnStop.Name = "_btnStop";
        _btnStop.Size = new Size(140, 44);
        _btnStop.TabIndex = 1;
        _btnStop.Text = "DETENER";
        _btnStop.UseVisualStyleBackColor = false;
        // 
        // _panelActions
        // 
        _panelActions.AutoSize = true;
        _panelActions.Dock = DockStyle.Top;
        _panelActions.Location = new Point(29, 888);
        _panelActions.Margin = new Padding(9, 8, 9, 8);
        _panelActions.Name = "_panelActions";
        _panelActions.Size = new Size(200, 100);
        _panelActions.TabIndex = 2;
        // 
        // _panelHeader
        // 
        _panelHeader.BackColor = Color.FromArgb(5, 5, 5);
        _panelHeader.Controls.Add(_flpHeaderActions);
        _panelHeader.Controls.Add(_lblAppTitle);
        _panelHeader.Dock = DockStyle.Top;
        _panelHeader.Location = new Point(0, 0);
        _panelHeader.Margin = new Padding(0);
        _panelHeader.Name = "_panelHeader";
        _panelHeader.Size = new Size(1829, 74);
        _panelHeader.TabIndex = 1;
        // 
        // _flpHeaderActions
        // 
        _flpHeaderActions.AutoSize = true;
        _flpHeaderActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _flpHeaderActions.Controls.Add(_btnAdvanced);

        _flpHeaderActions.Controls.Add(_btnControlCenter);
        _flpHeaderActions.Controls.Add(_btnOpenLog);
        _flpHeaderActions.Controls.Add(_btnOpenScripts);
        _flpHeaderActions.Controls.Add(_btnRun);
        _flpHeaderActions.Controls.Add(_btnStop);
        _flpHeaderActions.Dock = DockStyle.Right;
        _flpHeaderActions.Location = new Point(660, 0);
        _flpHeaderActions.Name = "_flpHeaderActions";
        _flpHeaderActions.Padding = new Padding(0, 14, 12, 14);
        _flpHeaderActions.Size = new Size(1169, 74);
        _flpHeaderActions.TabIndex = 2;
        _flpHeaderActions.WrapContents = false;
        // 
        // _lblAppTitle
        // 
        _lblAppTitle.AutoEllipsis = true;
        _lblAppTitle.Dock = DockStyle.Left;
        _lblAppTitle.Font = new Font("Consolas", 15F, FontStyle.Bold);
        _lblAppTitle.ForeColor = Color.FromArgb(0, 255, 65);
        _lblAppTitle.Location = new Point(0, 0);
        _lblAppTitle.Name = "_lblAppTitle";
        _lblAppTitle.Padding = new Padding(20, 15, 0, 0);
        _lblAppTitle.Size = new Size(340, 74);
        _lblAppTitle.TabIndex = 0;
        _lblAppTitle.Text = "> HOLOS_MIGRATOR";
        _lblAppTitle.TextAlign = ContentAlignment.TopLeft;
        // 
        // MainShellForm
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(9, 9, 11);
        ClientSize = new Size(1829, 1050);
        Controls.Add(_pnlMainContent);
        Controls.Add(_pnlSidebar);
        Controls.Add(_panelHeader);
        Margin = new Padding(4, 5, 4, 5);
        MinimumSize = new Size(1000, 700);
        Name = "MainShellForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Holos Migrator";
        _pnlMainContent.ResumeLayout(false);
        _pnlOperations.ResumeLayout(false);
        _tableRoot.ResumeLayout(false);
        _tableTop.ResumeLayout(false);
        _grpGeneral.ResumeLayout(false);
        _grpGeneral.PerformLayout();
        _tblGeneral.ResumeLayout(false);
        _tblGeneral.PerformLayout();
        _grpSsh.ResumeLayout(false);
        _grpSsh.PerformLayout();
        _tblSsh.ResumeLayout(false);
        _tblSsh.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_numSshPort).EndInit();
        _panelSshPassword.ResumeLayout(false);
        _panelSshPassword.PerformLayout();
        _grpOps.ResumeLayout(false);
        _grpOps.PerformLayout();
        _tblOps.ResumeLayout(false);
        _tblOps.PerformLayout();
        _grpLog.ResumeLayout(false);
        _grpLog.PerformLayout();
        _panelStatus.ResumeLayout(false);
        _panelHeader.ResumeLayout(false);
        _panelHeader.PerformLayout();
        _flpHeaderActions.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private ComboBox _cmbDeployTarget;
    private ComboBox _cmbMigrationMode;
    private TextBox _txtTenant;
    private FlowLayoutPanel _panelSkipFlags;
}
