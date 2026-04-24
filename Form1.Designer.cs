namespace HolosMigratorUI;

partial class Form1
{
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

    private TextBox _txtServerHost;
    private TextBox _txtServerUser;
    private NumericUpDown _numSshPort;
    private ComboBox _cmbSshAuth;
    private Panel _panelSshPassword;
    private TextBox _txtSshPassword;
    private Button _btnShowPassword;
    private Panel _panelHeader;
    private Label _lblAppTitle;
    private FlowLayoutPanel _flpHeaderActions;
    private TextBox _txtSshKeyPath;
    private CheckBox _chkRememberSshPassword;
    private CheckBox _chkSshBatchMode;
    private CheckBox _chkInteractiveWindowForPassword;
    private CheckBox _chkSkipPull;
    private CheckBox _chkSkipMigrations;
    private CheckBox _chkSkipBuild;
    private CheckBox _chkSkipPublicChecks;

    private Button _btnRun;
    private Button _btnStop;
    private Button _btnOpenScripts;
    private TextBox _txtLog;

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
        _tableRoot = new TableLayoutPanel();
        _tableTop = new TableLayoutPanel();
        _grpGeneral = new GroupBox();
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
        _btnShowPassword = new Button();
        _chkRememberSshPassword = new CheckBox();
        _lblSshKeyPath = new Label();
        _txtSshKeyPath = new TextBox();
        _chkSshBatchMode = new CheckBox();
        _chkInteractiveWindowForPassword = new CheckBox();
        _grpOps = new GroupBox();
        _tblOps = new TableLayoutPanel();
        _lblDeployTarget = new Label();
        _lblMigrationMode = new Label();
        _lblTenant = new Label();
        _chkSkipPull = new CheckBox();
        _chkSkipMigrations = new CheckBox();
        _chkSkipBuild = new CheckBox();
        _chkSkipPublicChecks = new CheckBox();
        _grpLog = new GroupBox();
        _txtLog = new TextBox();
        _panelActions = new FlowLayoutPanel();
        _btnRun = new Button();
        _btnStop = new Button();
        _btnOpenScripts = new Button();
        _panelHeader = new Panel();
        _flpHeaderActions = new FlowLayoutPanel();
        _lblAppTitle = new Label();
        _panelSkipFlags = new FlowLayoutPanel();
        _txtTenant = new TextBox();
        _cmbMigrationMode = new ComboBox();
        _cmbDeployTarget = new ComboBox();
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
        _panelHeader.SuspendLayout();
        SuspendLayout();
        // 
        // _tableRoot
        // 
        _tableRoot.ColumnCount = 1;
        _tableRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tableRoot.Controls.Add(_tableTop, 0, 0);
        _tableRoot.Controls.Add(_grpLog, 0, 1);
        _tableRoot.Dock = DockStyle.Fill;
        _tableRoot.Location = new Point(0, 64);
        _tableRoot.Margin = new Padding(4, 5, 4, 5);
        _tableRoot.Name = "_tableRoot";
        _tableRoot.Padding = new Padding(20, 16, 20, 16);
        _tableRoot.RowCount = 2;
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        _tableRoot.Size = new Size(1829, 1303);
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
        _tableTop.Size = new Size(1781, 561);
        _tableTop.TabIndex = 0;
        // 
        // _grpGeneral
        // 
        _grpGeneral.Controls.Add(_chkSkipMigrations);
        _grpGeneral.Controls.Add(_chkSkipPull);
        _grpGeneral.Controls.Add(_btnRun);
        _grpGeneral.Controls.Add(_chkSkipBuild);
        _grpGeneral.Controls.Add(_btnOpenScripts);
        _grpGeneral.Controls.Add(_chkSkipPublicChecks);
        _grpGeneral.Controls.Add(_tblGeneral);
        _grpGeneral.Dock = DockStyle.Fill;
        _grpGeneral.ForeColor = Color.FromArgb(37, 99, 235);
        _grpGeneral.Location = new Point(9, 10);
        _grpGeneral.Margin = new Padding(9, 10, 9, 10);
        _grpGeneral.Name = "_grpGeneral";
        _grpGeneral.Padding = new Padding(10, 12, 10, 10);
        _grpGeneral.Size = new Size(587, 541);
        _grpGeneral.TabIndex = 0;
        _grpGeneral.TabStop = false;
        _grpGeneral.Text = "General";
        // 
        // _tblGeneral
        // 
        _tblGeneral.AutoSize = true;
        _tblGeneral.AutoSizeMode = AutoSizeMode.GrowAndShrink;
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
        _tblGeneral.Dock = DockStyle.Top;
        _tblGeneral.ForeColor = Color.FromArgb(55, 65, 81);
        _tblGeneral.Location = new Point(10, 36);
        _tblGeneral.Margin = new Padding(4, 5, 4, 5);
        _tblGeneral.Name = "_tblGeneral";
        _tblGeneral.RowCount = 5;
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.RowStyles.Add(new RowStyle());
        _tblGeneral.Size = new Size(567, 197);
        _tblGeneral.TabIndex = 0;
        // 
        // _lblAction
        // 
        _lblAction.Anchor = AnchorStyles.Left;
        _lblAction.AutoSize = true;
        _lblAction.Location = new Point(4, 8);
        _lblAction.Margin = new Padding(4, 6, 4, 6);
        _lblAction.Name = "_lblAction";
        _lblAction.Size = new Size(69, 25);
        _lblAction.TabIndex = 0;
        _lblAction.Text = "Acción:";
        // 
        // _cmbAction
        // 
        _cmbAction.Dock = DockStyle.Fill;
        _cmbAction.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbAction.Items.AddRange(new object[] { "Deploy completo", "Solo migraciones" });
        _cmbAction.Location = new Point(247, 4);
        _cmbAction.Margin = new Padding(4);
        _cmbAction.Name = "_cmbAction";
        _cmbAction.Size = new Size(316, 33);
        _cmbAction.TabIndex = 1;
        // 
        // _lblRepoPath
        // 
        _lblRepoPath.Anchor = AnchorStyles.Left;
        _lblRepoPath.AutoSize = true;
        _lblRepoPath.Location = new Point(4, 48);
        _lblRepoPath.Margin = new Padding(4, 6, 4, 6);
        _lblRepoPath.Name = "_lblRepoPath";
        _lblRepoPath.Size = new Size(98, 25);
        _lblRepoPath.TabIndex = 2;
        _lblRepoPath.Text = "Repo local:";
        // 
        // _txtRepoPath
        // 
        _txtRepoPath.Dock = DockStyle.Fill;
        _txtRepoPath.Location = new Point(247, 45);
        _txtRepoPath.Margin = new Padding(4);
        _txtRepoPath.Name = "_txtRepoPath";
        _txtRepoPath.Size = new Size(316, 31);
        _txtRepoPath.TabIndex = 3;
        _txtRepoPath.Text = "C:\\Repos\\OmniSuite";
        // 
        // _lblRemoteRepoPath
        // 
        _lblRemoteRepoPath.Anchor = AnchorStyles.Left;
        _lblRemoteRepoPath.AutoSize = true;
        _lblRemoteRepoPath.Location = new Point(4, 87);
        _lblRemoteRepoPath.Margin = new Padding(4, 6, 4, 6);
        _lblRemoteRepoPath.Name = "_lblRemoteRepoPath";
        _lblRemoteRepoPath.Size = new Size(121, 25);
        _lblRemoteRepoPath.TabIndex = 4;
        _lblRemoteRepoPath.Text = "Repo remoto:";
        // 
        // _txtRemoteRepoPath
        // 
        _txtRemoteRepoPath.Dock = DockStyle.Fill;
        _txtRemoteRepoPath.Location = new Point(247, 84);
        _txtRemoteRepoPath.Margin = new Padding(4);
        _txtRemoteRepoPath.Name = "_txtRemoteRepoPath";
        _txtRemoteRepoPath.Size = new Size(316, 31);
        _txtRemoteRepoPath.TabIndex = 5;
        _txtRemoteRepoPath.Text = "/root/OmniSuite";
        // 
        // _lblBranch
        // 
        _lblBranch.Anchor = AnchorStyles.Left;
        _lblBranch.AutoSize = true;
        _lblBranch.Location = new Point(4, 126);
        _lblBranch.Margin = new Padding(4, 6, 4, 6);
        _lblBranch.Name = "_lblBranch";
        _lblBranch.Size = new Size(61, 25);
        _lblBranch.TabIndex = 6;
        _lblBranch.Text = "Rama:";
        // 
        // _txtBranch
        // 
        _txtBranch.Dock = DockStyle.Fill;
        _txtBranch.Location = new Point(247, 123);
        _txtBranch.Margin = new Padding(4);
        _txtBranch.Name = "_txtBranch";
        _txtBranch.Size = new Size(316, 31);
        _txtBranch.TabIndex = 7;
        _txtBranch.Text = "develop";
        // 
        // _lblComposeFile
        // 
        _lblComposeFile.Anchor = AnchorStyles.Left;
        _lblComposeFile.AutoSize = true;
        _lblComposeFile.Location = new Point(4, 165);
        _lblComposeFile.Margin = new Padding(4, 6, 4, 6);
        _lblComposeFile.Name = "_lblComposeFile";
        _lblComposeFile.Size = new Size(155, 25);
        _lblComposeFile.TabIndex = 8;
        _lblComposeFile.Text = "Archivo compose:";
        // 
        // _txtComposeFile
        // 
        _txtComposeFile.Dock = DockStyle.Fill;
        _txtComposeFile.Location = new Point(247, 162);
        _txtComposeFile.Margin = new Padding(4);
        _txtComposeFile.Name = "_txtComposeFile";
        _txtComposeFile.Size = new Size(316, 31);
        _txtComposeFile.TabIndex = 9;
        _txtComposeFile.Text = "docker-compose.hostinger.yml";
        // 
        // _grpSsh
        // 
        _grpSsh.Controls.Add(_tblSsh);
        _grpSsh.Dock = DockStyle.Fill;
        _grpSsh.ForeColor = Color.FromArgb(37, 99, 235);
        _grpSsh.Location = new Point(614, 10);
        _grpSsh.Margin = new Padding(9, 10, 9, 10);
        _grpSsh.Name = "_grpSsh";
        _grpSsh.Padding = new Padding(10, 12, 10, 10);
        _grpSsh.Size = new Size(623, 541);
        _grpSsh.TabIndex = 1;
        _grpSsh.TabStop = false;
        _grpSsh.Text = "SSH";
        // 
        // _tblSsh
        // 
        _tblSsh.AutoSize = true;
        _tblSsh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
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
        _tblSsh.ForeColor = Color.FromArgb(55, 65, 81);
        _tblSsh.Location = new Point(10, 36);
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
        _tblSsh.Size = new Size(603, 371);
        _tblSsh.TabIndex = 0;
        // 
        // _lblServerHost
        // 
        _lblServerHost.Anchor = AnchorStyles.Left;
        _lblServerHost.AutoSize = true;
        _lblServerHost.Location = new Point(4, 10);
        _lblServerHost.Margin = new Padding(4, 6, 4, 6);
        _lblServerHost.Name = "_lblServerHost";
        _lblServerHost.Size = new Size(82, 25);
        _lblServerHost.TabIndex = 0;
        _lblServerHost.Text = "Servidor:";
        // 
        // _txtServerHost
        // 
        _txtServerHost.Dock = DockStyle.Fill;
        _txtServerHost.Location = new Point(247, 7);
        _txtServerHost.Margin = new Padding(4, 7, 4, 7);
        _txtServerHost.Name = "_txtServerHost";
        _txtServerHost.Size = new Size(352, 31);
        _txtServerHost.TabIndex = 1;
        // 
        // _lblServerUser
        // 
        _lblServerUser.Anchor = AnchorStyles.Left;
        _lblServerUser.AutoSize = true;
        _lblServerUser.Location = new Point(4, 55);
        _lblServerUser.Margin = new Padding(4, 6, 4, 6);
        _lblServerUser.Name = "_lblServerUser";
        _lblServerUser.Size = new Size(76, 25);
        _lblServerUser.TabIndex = 2;
        _lblServerUser.Text = "Usuario:";
        // 
        // _txtServerUser
        // 
        _txtServerUser.Dock = DockStyle.Fill;
        _txtServerUser.Location = new Point(247, 52);
        _txtServerUser.Margin = new Padding(4, 7, 4, 7);
        _txtServerUser.Name = "_txtServerUser";
        _txtServerUser.Size = new Size(352, 31);
        _txtServerUser.TabIndex = 3;
        // 
        // _lblSshPort
        // 
        _lblSshPort.Anchor = AnchorStyles.Left;
        _lblSshPort.AutoSize = true;
        _lblSshPort.Location = new Point(4, 100);
        _lblSshPort.Margin = new Padding(4, 6, 4, 6);
        _lblSshPort.Name = "_lblSshPort";
        _lblSshPort.Size = new Size(106, 25);
        _lblSshPort.TabIndex = 4;
        _lblSshPort.Text = "Puerto SSH:";
        // 
        // _numSshPort
        // 
        _numSshPort.Dock = DockStyle.Fill;
        _numSshPort.Location = new Point(247, 97);
        _numSshPort.Margin = new Padding(4, 7, 4, 7);
        _numSshPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        _numSshPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        _numSshPort.Name = "_numSshPort";
        _numSshPort.Size = new Size(352, 31);
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
        _lblSshAuth.Size = new Size(103, 25);
        _lblSshAuth.TabIndex = 6;
        _lblSshAuth.Text = "Modo SSH:";
        // 
        // _cmbSshAuth
        // 
        _cmbSshAuth.Dock = DockStyle.Fill;
        _cmbSshAuth.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbSshAuth.Items.AddRange(new object[] { "Auto - Llave si existe, si no password", "Key - Solo llave SSH", "Password - Pedir clave SSH" });
        _cmbSshAuth.Location = new Point(247, 139);
        _cmbSshAuth.Margin = new Padding(4);
        _cmbSshAuth.Name = "_cmbSshAuth";
        _cmbSshAuth.Size = new Size(352, 33);
        _cmbSshAuth.TabIndex = 7;
        // 
        // _lblSshPassword
        // 
        _lblSshPassword.Anchor = AnchorStyles.Left;
        _lblSshPassword.AutoSize = true;
        _lblSshPassword.Location = new Point(4, 186);
        _lblSshPassword.Margin = new Padding(4, 6, 4, 6);
        _lblSshPassword.Name = "_lblSshPassword";
        _lblSshPassword.Size = new Size(129, 25);
        _lblSshPassword.TabIndex = 8;
        _lblSshPassword.Text = "Password SSH:";
        // 
        // _panelSshPassword
        // 
        _panelSshPassword.Controls.Add(_txtSshPassword);
        _panelSshPassword.Controls.Add(_btnShowPassword);
        _panelSshPassword.Dock = DockStyle.Fill;
        _panelSshPassword.Location = new Point(247, 180);
        _panelSshPassword.Margin = new Padding(4);
        _panelSshPassword.Name = "_panelSshPassword";
        _panelSshPassword.Size = new Size(352, 37);
        _panelSshPassword.TabIndex = 9;
        // 
        // _txtSshPassword
        // 
        _txtSshPassword.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _txtSshPassword.Location = new Point(0, 0);
        _txtSshPassword.Name = "_txtSshPassword";
        _txtSshPassword.Size = new Size(351, 31);
        _txtSshPassword.TabIndex = 0;
        _txtSshPassword.UseSystemPasswordChar = true;
        // 
        // _btnShowPassword
        // 
        _btnShowPassword.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        _btnShowPassword.Cursor = Cursors.Hand;
        _btnShowPassword.FlatAppearance.BorderSize = 0;
        _btnShowPassword.FlatStyle = FlatStyle.Flat;
        _btnShowPassword.Location = new Point(316, 0);
        _btnShowPassword.Name = "_btnShowPassword";
        _btnShowPassword.Size = new Size(36, 37);
        _btnShowPassword.TabIndex = 1;
        _btnShowPassword.TabStop = false;
        _btnShowPassword.Text = "👁";
        // 
        // _chkRememberSshPassword
        // 
        _chkRememberSshPassword.AutoSize = true;
        _chkRememberSshPassword.Location = new Point(247, 225);
        _chkRememberSshPassword.Margin = new Padding(4);
        _chkRememberSshPassword.Name = "_chkRememberSshPassword";
        _chkRememberSshPassword.Size = new Size(258, 29);
        _chkRememberSshPassword.TabIndex = 10;
        _chkRememberSshPassword.Text = "Recordar clave SSH (cifrada)";
        // 
        // _lblSshKeyPath
        // 
        _lblSshKeyPath.Anchor = AnchorStyles.Left;
        _lblSshKeyPath.AutoSize = true;
        _lblSshKeyPath.Location = new Point(4, 265);
        _lblSshKeyPath.Margin = new Padding(4, 6, 4, 6);
        _lblSshKeyPath.Name = "_lblSshKeyPath";
        _lblSshKeyPath.Size = new Size(92, 25);
        _lblSshKeyPath.TabIndex = 11;
        _lblSshKeyPath.Text = "Ruta llave:";
        // 
        // _txtSshKeyPath
        // 
        _txtSshKeyPath.Dock = DockStyle.Fill;
        _txtSshKeyPath.Location = new Point(247, 262);
        _txtSshKeyPath.Margin = new Padding(4);
        _txtSshKeyPath.Name = "_txtSshKeyPath";
        _txtSshKeyPath.Size = new Size(352, 31);
        _txtSshKeyPath.TabIndex = 12;
        _txtSshKeyPath.Text = "C:\\Users\\USER\\.ssh\\id_ed25519";
        // 
        // _chkSshBatchMode
        // 
        _chkSshBatchMode.AutoSize = true;
        _chkSshBatchMode.Location = new Point(247, 301);
        _chkSshBatchMode.Margin = new Padding(4);
        _chkSshBatchMode.Name = "_chkSshBatchMode";
        _chkSshBatchMode.Size = new Size(267, 29);
        _chkSshBatchMode.TabIndex = 13;
        _chkSshBatchMode.Text = "SshBatchMode (sin prompts)";
        // 
        // _chkInteractiveWindowForPassword
        // 
        _chkInteractiveWindowForPassword.AutoSize = true;
        _chkInteractiveWindowForPassword.Checked = true;
        _chkInteractiveWindowForPassword.CheckState = CheckState.Checked;
        _chkInteractiveWindowForPassword.Location = new Point(247, 338);
        _chkInteractiveWindowForPassword.Margin = new Padding(4);
        _chkInteractiveWindowForPassword.Name = "_chkInteractiveWindowForPassword";
        _chkInteractiveWindowForPassword.Size = new Size(351, 29);
        _chkInteractiveWindowForPassword.TabIndex = 14;
        _chkInteractiveWindowForPassword.Text = "Abrir terminal interactiva para Password";
        // 
        // _grpOps
        // 
        _grpOps.Controls.Add(_tblOps);
        _grpOps.Dock = DockStyle.Fill;
        _grpOps.ForeColor = Color.FromArgb(37, 99, 235);
        _grpOps.Location = new Point(1255, 10);
        _grpOps.Margin = new Padding(9, 10, 9, 10);
        _grpOps.Name = "_grpOps";
        _grpOps.Padding = new Padding(10, 12, 10, 10);
        _grpOps.Size = new Size(517, 541);
        _grpOps.TabIndex = 1;
        _grpOps.TabStop = false;
        _grpOps.Text = "Opciones de despliegue y migración";
        // 
        // _tblOps
        // 
        _tblOps.AutoSize = true;
        _tblOps.AutoSizeMode = AutoSizeMode.GrowAndShrink;
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
        _tblOps.ForeColor = Color.FromArgb(55, 65, 81);
        _tblOps.Location = new Point(10, 36);
        _tblOps.Margin = new Padding(4, 5, 4, 5);
        _tblOps.Name = "_tblOps";
        _tblOps.RowCount = 4;
        _tblOps.RowStyles.Add(new RowStyle());
        _tblOps.RowStyles.Add(new RowStyle());
        _tblOps.RowStyles.Add(new RowStyle());
        _tblOps.RowStyles.Add(new RowStyle());
        _tblOps.Size = new Size(497, 194);
        _tblOps.TabIndex = 0;
        // 
        // _lblDeployTarget
        // 
        _lblDeployTarget.Anchor = AnchorStyles.Left;
        _lblDeployTarget.AutoSize = true;
        _lblDeployTarget.Location = new Point(4, 6);
        _lblDeployTarget.Margin = new Padding(4, 6, 4, 6);
        _lblDeployTarget.Name = "_lblDeployTarget";
        _lblDeployTarget.Size = new Size(85, 50);
        _lblDeployTarget.TabIndex = 0;
        _lblDeployTarget.Text = "Objetivo deploy:";
        // 
        // _lblMigrationMode
        // 
        _lblMigrationMode.Anchor = AnchorStyles.Left;
        _lblMigrationMode.AutoSize = true;
        _lblMigrationMode.Location = new Point(4, 68);
        _lblMigrationMode.Margin = new Padding(4, 6, 4, 6);
        _lblMigrationMode.Name = "_lblMigrationMode";
        _lblMigrationMode.Size = new Size(95, 50);
        _lblMigrationMode.TabIndex = 2;
        _lblMigrationMode.Text = "Modo migración:";
        // 
        // _lblTenant
        // 
        _lblTenant.Anchor = AnchorStyles.Left;
        _lblTenant.AutoSize = true;
        _lblTenant.Location = new Point(4, 130);
        _lblTenant.Margin = new Padding(4, 6, 4, 6);
        _lblTenant.Name = "_lblTenant";
        _lblTenant.Size = new Size(112, 50);
        _lblTenant.TabIndex = 4;
        _lblTenant.Text = "Tenant (solo U):";
        // 
        // _chkSkipPull
        // 
        _chkSkipPull.AutoSize = true;
        _chkSkipPull.Location = new Point(28, 480);
        _chkSkipPull.Margin = new Padding(0, 4, 16, 4);
        _chkSkipPull.Name = "_chkSkipPull";
        _chkSkipPull.Size = new Size(122, 29);
        _chkSkipPull.TabIndex = 0;
        _chkSkipPull.Text = "Omitir pull";
        // 
        // _chkSkipMigrations
        // 
        _chkSkipMigrations.AutoSize = true;
        _chkSkipMigrations.Location = new Point(282, 406);
        _chkSkipMigrations.Margin = new Padding(0, 4, 16, 4);
        _chkSkipMigrations.Name = "_chkSkipMigrations";
        _chkSkipMigrations.Size = new Size(189, 29);
        _chkSkipMigrations.TabIndex = 1;
        _chkSkipMigrations.Text = "Omitir migraciones";
        // 
        // _chkSkipBuild
        // 
        _chkSkipBuild.AutoSize = true;
        _chkSkipBuild.Location = new Point(28, 443);
        _chkSkipBuild.Margin = new Padding(0, 4, 16, 4);
        _chkSkipBuild.Name = "_chkSkipBuild";
        _chkSkipBuild.Size = new Size(133, 29);
        _chkSkipBuild.TabIndex = 2;
        _chkSkipBuild.Text = "Omitir build";
        // 
        // _chkSkipPublicChecks
        // 
        _chkSkipPublicChecks.AutoSize = true;
        _chkSkipPublicChecks.Location = new Point(28, 406);
        _chkSkipPublicChecks.Margin = new Padding(0, 4, 0, 4);
        _chkSkipPublicChecks.Name = "_chkSkipPublicChecks";
        _chkSkipPublicChecks.Size = new Size(217, 29);
        _chkSkipPublicChecks.TabIndex = 3;
        _chkSkipPublicChecks.Text = "Omitir checks públicos";
        // 
        // _grpLog
        // 
        _grpLog.Controls.Add(_btnStop);
        _grpLog.Controls.Add(_txtLog);
        _grpLog.Dock = DockStyle.Fill;
        _grpLog.ForeColor = Color.FromArgb(37, 99, 235);
        _grpLog.Location = new Point(29, 597);
        _grpLog.Margin = new Padding(9, 10, 9, 10);
        _grpLog.Name = "_grpLog";
        _grpLog.Padding = new Padding(10, 12, 10, 10);
        _grpLog.Size = new Size(1771, 680);
        _grpLog.TabIndex = 3;
        _grpLog.TabStop = false;
        _grpLog.Text = "Salida en tiempo real";
        // 
        // _txtLog
        // 
        _txtLog.BackColor = Color.FromArgb(22, 27, 34);
        _txtLog.Dock = DockStyle.Fill;
        _txtLog.Font = new Font("Consolas", 10F);
        _txtLog.ForeColor = Color.FromArgb(201, 209, 217);
        _txtLog.Location = new Point(10, 36);
        _txtLog.Margin = new Padding(4, 5, 4, 5);
        _txtLog.Multiline = true;
        _txtLog.Name = "_txtLog";
        _txtLog.ReadOnly = true;
        _txtLog.ScrollBars = ScrollBars.Both;
        _txtLog.Size = new Size(1751, 634);
        _txtLog.TabIndex = 0;
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
        // _btnRun
        // 
        _btnRun.BackColor = Color.FromArgb(22, 163, 74);
        _btnRun.Cursor = Cursors.Hand;
        _btnRun.FlatAppearance.BorderSize = 0;
        _btnRun.FlatStyle = FlatStyle.Flat;
        _btnRun.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _btnRun.ForeColor = Color.White;
        _btnRun.Location = new Point(47, 322);
        _btnRun.Margin = new Padding(0, 10, 6, 10);
        _btnRun.Name = "_btnRun";
        _btnRun.Size = new Size(140, 44);
        _btnRun.TabIndex = 0;
        _btnRun.Text = "▶  Ejecutar";
        _btnRun.UseVisualStyleBackColor = false;
        // 
        // _btnStop
        // 
        _btnStop.BackColor = Color.FromArgb(220, 38, 38);
        _btnStop.Cursor = Cursors.Hand;
        _btnStop.Enabled = false;
        _btnStop.FlatAppearance.BorderSize = 0;
        _btnStop.FlatStyle = FlatStyle.Flat;
        _btnStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _btnStop.ForeColor = Color.White;
        _btnStop.Location = new Point(1583, 46);
        _btnStop.Margin = new Padding(0, 10, 6, 10);
        _btnStop.Name = "_btnStop";
        _btnStop.Size = new Size(140, 44);
        _btnStop.TabIndex = 1;
        _btnStop.Text = "■  Detener";
        _btnStop.UseVisualStyleBackColor = false;
        // 
        // _btnOpenScripts
        // 
        _btnOpenScripts.BackColor = Color.FromArgb(71, 85, 105);
        _btnOpenScripts.Cursor = Cursors.Hand;
        _btnOpenScripts.FlatAppearance.BorderSize = 0;
        _btnOpenScripts.FlatStyle = FlatStyle.Flat;
        _btnOpenScripts.Font = new Font("Segoe UI", 10F);
        _btnOpenScripts.ForeColor = Color.White;
        _btnOpenScripts.Location = new Point(47, 261);
        _btnOpenScripts.Margin = new Padding(0, 10, 10, 10);
        _btnOpenScripts.Name = "_btnOpenScripts";
        _btnOpenScripts.Size = new Size(140, 44);
        _btnOpenScripts.TabIndex = 2;
        _btnOpenScripts.Text = "📂  Scripts";
        _btnOpenScripts.UseVisualStyleBackColor = false;
        // 
        // _panelHeader
        // 
        _panelHeader.BackColor = Color.FromArgb(15, 23, 42);
        _panelHeader.Controls.Add(_flpHeaderActions);
        _panelHeader.Controls.Add(_lblAppTitle);
        _panelHeader.Dock = DockStyle.Top;
        _panelHeader.Location = new Point(0, 0);
        _panelHeader.Margin = new Padding(0);
        _panelHeader.Name = "_panelHeader";
        _panelHeader.Size = new Size(1829, 64);
        _panelHeader.TabIndex = 1;
        // 
        // _flpHeaderActions
        // 
        _flpHeaderActions.AutoSize = true;
        _flpHeaderActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _flpHeaderActions.Dock = DockStyle.Right;
        _flpHeaderActions.Location = new Point(1817, 0);
        _flpHeaderActions.Name = "_flpHeaderActions";
        _flpHeaderActions.Padding = new Padding(0, 10, 12, 10);
        _flpHeaderActions.Size = new Size(12, 64);
        _flpHeaderActions.TabIndex = 2;
        _flpHeaderActions.WrapContents = false;
        // 
        // _lblAppTitle
        // 
        _lblAppTitle.Dock = DockStyle.Fill;
        _lblAppTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        _lblAppTitle.ForeColor = Color.White;
        _lblAppTitle.Location = new Point(0, 0);
        _lblAppTitle.Name = "_lblAppTitle";
        _lblAppTitle.Padding = new Padding(24, 0, 0, 0);
        _lblAppTitle.Size = new Size(1829, 64);
        _lblAppTitle.TabIndex = 0;
        _lblAppTitle.Text = "Holos Migrator";
        _lblAppTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _panelSkipFlags
        // 
        _panelSkipFlags.AutoSize = true;
        _panelSkipFlags.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _panelSkipFlags.Dock = DockStyle.Fill;
        _panelSkipFlags.Location = new Point(140, 190);
        _panelSkipFlags.Margin = new Padding(0, 4, 0, 4);
        _panelSkipFlags.Name = "_panelSkipFlags";
        _panelSkipFlags.Size = new Size(357, 1);
        _panelSkipFlags.TabIndex = 6;
        // 
        // _txtTenant
        // 
        _txtTenant.Dock = DockStyle.Fill;
        _txtTenant.Location = new Point(144, 128);
        _txtTenant.Margin = new Padding(4);
        _txtTenant.Name = "_txtTenant";
        _txtTenant.Size = new Size(349, 31);
        _txtTenant.TabIndex = 5;
        // 
        // _cmbMigrationMode
        // 
        _cmbMigrationMode.Dock = DockStyle.Fill;
        _cmbMigrationMode.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbMigrationMode.Items.AddRange(new object[] { "B - Host + todos los tenants", "H - Solo Host", "T - Solo todos los tenants", "U - Un tenant especifico", "N - Ninguna" });
        _cmbMigrationMode.Location = new Point(144, 66);
        _cmbMigrationMode.Margin = new Padding(4);
        _cmbMigrationMode.Name = "_cmbMigrationMode";
        _cmbMigrationMode.Size = new Size(349, 33);
        _cmbMigrationMode.TabIndex = 3;
        // 
        // _cmbDeployTarget
        // 
        _cmbDeployTarget.Dock = DockStyle.Fill;
        _cmbDeployTarget.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbDeployTarget.Items.AddRange(new object[] { "Both - Front + API", "Backend - Solo API", "Frontend - Solo Front" });
        _cmbDeployTarget.Location = new Point(144, 4);
        _cmbDeployTarget.Margin = new Padding(4);
        _cmbDeployTarget.Name = "_cmbDeployTarget";
        _cmbDeployTarget.Size = new Size(349, 33);
        _cmbDeployTarget.TabIndex = 1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(240, 242, 248);
        ClientSize = new Size(1829, 1367);
        Controls.Add(_tableRoot);
        Controls.Add(_panelHeader);
        Margin = new Padding(4, 5, 4, 5);
        MinimumSize = new Size(1705, 1006);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Holos Migrator";
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
        _panelHeader.ResumeLayout(false);
        _panelHeader.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private ComboBox _cmbDeployTarget;
    private ComboBox _cmbMigrationMode;
    private TextBox _txtTenant;
    private FlowLayoutPanel _panelSkipFlags;
}
