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
    private FlowLayoutPanel _panelSkipFlags;

    private ComboBox _cmbAction;
    private TextBox _txtRepoPath;
    private TextBox _txtRemoteRepoPath;
    private TextBox _txtBranch;
    private TextBox _txtComposeFile;

    private TextBox _txtServerHost;
    private TextBox _txtServerUser;
    private NumericUpDown _numSshPort;
    private ComboBox _cmbSshAuth;
    private TextBox _txtSshPassword;
    private TextBox _txtSshKeyPath;
    private CheckBox _chkRememberSshPassword;
    private CheckBox _chkSshBatchMode;
    private CheckBox _chkInteractiveWindowForPassword;

    private ComboBox _cmbMigrationMode;
    private ComboBox _cmbDeployTarget;
    private TextBox _txtTenant;
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
        _grpSsh = new GroupBox();
        _tblSsh = new TableLayoutPanel();
        _grpOps = new GroupBox();
        _tblOps = new TableLayoutPanel();
        _panelSkipFlags = new FlowLayoutPanel();
        _panelActions = new FlowLayoutPanel();
        _grpLog = new GroupBox();
        _txtLog = new TextBox();

        _cmbAction = new ComboBox();
        _txtRepoPath = new TextBox();
        _txtRemoteRepoPath = new TextBox();
        _txtBranch = new TextBox();
        _txtComposeFile = new TextBox();

        _txtServerHost = new TextBox();
        _txtServerUser = new TextBox();
        _numSshPort = new NumericUpDown();
        _cmbSshAuth = new ComboBox();
        _txtSshPassword = new TextBox();
        _txtSshKeyPath = new TextBox();
        _chkRememberSshPassword = new CheckBox();
        _chkSshBatchMode = new CheckBox();
        _chkInteractiveWindowForPassword = new CheckBox();

        _cmbMigrationMode = new ComboBox();
        _cmbDeployTarget = new ComboBox();
        _txtTenant = new TextBox();
        _chkSkipPull = new CheckBox();
        _chkSkipMigrations = new CheckBox();
        _chkSkipBuild = new CheckBox();
        _chkSkipPublicChecks = new CheckBox();

        _btnRun = new Button();
        _btnStop = new Button();
        _btnOpenScripts = new Button();

        ((System.ComponentModel.ISupportInitialize)_numSshPort).BeginInit();
        SuspendLayout();

        // _tableRoot
        _tableRoot.ColumnCount = 1;
        _tableRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tableRoot.Dock = DockStyle.Fill;
        _tableRoot.Location = new Point(0, 0);
        _tableRoot.Name = "_tableRoot";
        _tableRoot.Padding = new Padding(14);
        _tableRoot.RowCount = 4;
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _tableRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tableRoot.Size = new Size(1184, 761);

        // _tableTop
        _tableTop.AutoSize = true;
        _tableTop.ColumnCount = 2;
        _tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _tableTop.Dock = DockStyle.Fill;
        _tableTop.RowCount = 1;
        _tableTop.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        // _grpGeneral
        _grpGeneral.Text = "General";
        _grpGeneral.AutoSize = true;
        _grpGeneral.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpGeneral.Dock = DockStyle.Fill;
        _grpGeneral.Padding = new Padding(10);
        _grpGeneral.Margin = new Padding(6);
        _grpGeneral.Controls.Add(_tblGeneral);

        // _tblGeneral
        _tblGeneral.AutoSize = true;
        _tblGeneral.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _tblGeneral.ColumnCount = 2;
        _tblGeneral.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        _tblGeneral.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tblGeneral.Dock = DockStyle.Fill;

        AddLabeledControl(_tblGeneral, 0, "Acción:", _cmbAction);
        AddLabeledControl(_tblGeneral, 1, "Repo local:", _txtRepoPath);
        AddLabeledControl(_tblGeneral, 2, "Repo remoto:", _txtRemoteRepoPath);
        AddLabeledControl(_tblGeneral, 3, "Rama:", _txtBranch);
        AddLabeledControl(_tblGeneral, 4, "Archivo compose:", _txtComposeFile);

        _cmbAction.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbAction.Items.AddRange(new object[] { "Deploy completo", "Solo migraciones" });
        _cmbAction.SelectedIndex = 0;

        _txtRepoPath.Text = @"C:\Repos\OmniSuite";
        _txtRemoteRepoPath.Text = "/root/OmniSuite";
        _txtBranch.Text = "develop";
        _txtComposeFile.Text = "docker-compose.hostinger.yml";

        // _grpSsh
        _grpSsh.Text = "SSH";
        _grpSsh.AutoSize = true;
        _grpSsh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpSsh.Dock = DockStyle.Fill;
        _grpSsh.Padding = new Padding(10);
        _grpSsh.Margin = new Padding(6);
        _grpSsh.Controls.Add(_tblSsh);

        // _tblSsh
        _tblSsh.AutoSize = true;
        _tblSsh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _tblSsh.ColumnCount = 2;
        _tblSsh.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        _tblSsh.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tblSsh.Dock = DockStyle.Fill;

        AddLabeledControl(_tblSsh, 0, "Servidor:", _txtServerHost);
        AddLabeledControl(_tblSsh, 1, "Usuario:", _txtServerUser);
        AddLabeledControl(_tblSsh, 2, "Puerto SSH:", _numSshPort);
        AddLabeledControl(_tblSsh, 3, "Modo SSH:", _cmbSshAuth);
        AddLabeledControl(_tblSsh, 4, "Clave SSH:", _txtSshPassword);
        AddWideControl(_tblSsh, 5, _chkRememberSshPassword);
        AddLabeledControl(_tblSsh, 6, "Ruta llave:", _txtSshKeyPath);
        AddWideControl(_tblSsh, 7, _chkSshBatchMode);
        AddWideControl(_tblSsh, 8, _chkInteractiveWindowForPassword);

        _txtServerHost.Text = "76.13.37.115";
        _txtServerUser.Text = "root";
        _numSshPort.Minimum = 1;
        _numSshPort.Maximum = 65535;
        _numSshPort.Value = 22;

        _cmbSshAuth.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbSshAuth.Items.AddRange(new object[]
        {
            "Auto - Llave si existe, si no password",
            "Key - Solo llave SSH",
            "Password - Pedir clave SSH"
        });
        _cmbSshAuth.SelectedIndex = 0;

        _txtSshPassword.UseSystemPasswordChar = true;

        _chkRememberSshPassword.Text = "Recordar clave SSH en este equipo (cifrada)";

        _txtSshKeyPath.Text = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\.ssh\id_ed25519");

        _chkSshBatchMode.Text = "SshBatchMode (sin prompts)";
        _chkInteractiveWindowForPassword.Text = "Abrir terminal interactiva para Password";
        _chkInteractiveWindowForPassword.Checked = true;

        // _grpOps
        _grpOps.Text = "Opciones de despliegue y migración";
        _grpOps.AutoSize = true;
        _grpOps.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpOps.Dock = DockStyle.Fill;
        _grpOps.Padding = new Padding(10);
        _grpOps.Margin = new Padding(6);
        _grpOps.Controls.Add(_tblOps);

        // _tblOps
        _tblOps.AutoSize = true;
        _tblOps.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _tblOps.ColumnCount = 2;
        _tblOps.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        _tblOps.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tblOps.Dock = DockStyle.Fill;

        AddLabeledControl(_tblOps, 0, "Objetivo deploy:", _cmbDeployTarget);
        AddLabeledControl(_tblOps, 1, "Modo migración:", _cmbMigrationMode);
        AddLabeledControl(_tblOps, 2, "Tenant (solo U):", _txtTenant);
        AddWideControl(_tblOps, 3, _panelSkipFlags);

        _cmbDeployTarget.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbDeployTarget.Items.AddRange(new object[]
        {
            "Both - Front + API",
            "Backend - Solo API",
            "Frontend - Solo Front"
        });
        _cmbDeployTarget.SelectedIndex = 0;

        _cmbMigrationMode.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbMigrationMode.Items.AddRange(new object[]
        {
            "B - Host + todos los tenants",
            "H - Solo Host",
            "T - Solo todos los tenants",
            "U - Un tenant especifico",
            "N - Ninguna"
        });
        _cmbMigrationMode.SelectedIndex = 0;

        _chkSkipPull.Text = "Omitir pull";
        _chkSkipMigrations.Text = "Omitir migraciones";
        _chkSkipBuild.Text = "Omitir build";
        _chkSkipPublicChecks.Text = "Omitir checks públicos";

        _panelSkipFlags.AutoSize = true;
        _panelSkipFlags.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _panelSkipFlags.Dock = DockStyle.Fill;
        _panelSkipFlags.FlowDirection = FlowDirection.LeftToRight;
        _panelSkipFlags.WrapContents = true;
        _panelSkipFlags.Margin = new Padding(0, 4, 0, 4);
        _panelSkipFlags.Controls.Add(_chkSkipPull);
        _panelSkipFlags.Controls.Add(_chkSkipMigrations);
        _panelSkipFlags.Controls.Add(_chkSkipBuild);
        _panelSkipFlags.Controls.Add(_chkSkipPublicChecks);

        _chkSkipPull.Margin = new Padding(0, 4, 18, 4);
        _chkSkipMigrations.Margin = new Padding(0, 4, 18, 4);
        _chkSkipBuild.Margin = new Padding(0, 4, 18, 4);
        _chkSkipPublicChecks.Margin = new Padding(0, 4, 0, 4);

        // _panelActions
        _panelActions.AutoSize = true;
        _panelActions.FlowDirection = FlowDirection.LeftToRight;
        _panelActions.Dock = DockStyle.Top;
        _panelActions.Margin = new Padding(6);

        _btnRun.Text = "Ejecutar ahora";
        _btnRun.Width = 120;
        _btnRun.Height = 36;

        _btnStop.Text = "Detener proceso";
        _btnStop.Width = 120;
        _btnStop.Height = 36;
        _btnStop.Enabled = false;

        _btnOpenScripts.Text = "Abrir scripts";
        _btnOpenScripts.Width = 170;
        _btnOpenScripts.Height = 36;

        _panelActions.Controls.Add(_btnRun);
        _panelActions.Controls.Add(_btnStop);
        _panelActions.Controls.Add(_btnOpenScripts);

        // _grpLog
        _grpLog.Text = "Salida en tiempo real";
        _grpLog.Dock = DockStyle.Fill;
        _grpLog.Padding = new Padding(10);
        _grpLog.Margin = new Padding(6);

        _txtLog.Dock = DockStyle.Fill;
        _txtLog.Multiline = true;
        _txtLog.ScrollBars = ScrollBars.Both;
        _txtLog.ReadOnly = true;
        _txtLog.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
        _txtLog.BackColor = Color.White;
        _grpLog.Controls.Add(_txtLog);

        // compose main tree
        _tableTop.Controls.Add(_grpGeneral, 0, 0);
        _tableTop.Controls.Add(_grpSsh, 1, 0);

        _tableRoot.Controls.Add(_tableTop, 0, 0);
        _tableRoot.Controls.Add(_grpOps, 0, 1);
        _tableRoot.Controls.Add(_panelActions, 0, 2);
        _tableRoot.Controls.Add(_grpLog, 0, 3);

        Controls.Add(_tableRoot);

        // Form1
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoScroll = true;
        BackColor = Color.FromArgb(246, 248, 251);
        ClientSize = new Size(1280, 820);
        MinimumSize = new Size(1200, 780);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Holos Migrator UI";

        ((System.ComponentModel.ISupportInitialize)_numSshPort).EndInit();
        ResumeLayout(false);
    }

    private static void AddLabeledControl(TableLayoutPanel table, int row, string text, Control control)
    {
        while (table.RowStyles.Count <= row)
        {
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        table.RowCount = Math.Max(table.RowCount, row + 1);

        var label = new Label
        {
            Text = text,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(3, 8, 3, 8)
        };

        control.Dock = DockStyle.Fill;
        control.Margin = new Padding(3, 4, 3, 4);

        table.Controls.Add(label, 0, row);
        table.Controls.Add(control, 1, row);
    }

    private static void AddWideControl(TableLayoutPanel table, int row, Control control)
    {
        while (table.RowStyles.Count <= row)
        {
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        table.RowCount = Math.Max(table.RowCount, row + 1);

        control.Dock = DockStyle.Fill;
        control.Margin = new Padding(3, 4, 3, 4);

        table.Controls.Add(control, 1, row);
    }

    #endregion
}
