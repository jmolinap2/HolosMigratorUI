using System.Data;
using HolosMigratorUI.Core;
using System.Text;
using System.ComponentModel;

namespace HolosMigratorUI.UI;

public class LogCenterUserControl : UserControl
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private readonly Func<string> _hostProvider;
    private readonly Func<int> _sshPortProvider;
    private readonly Func<string> _serverUserProvider;
    private readonly Func<string> _sshKeyPathProvider;
    private readonly Func<string> _sshPasswordProvider;

    private DataGridView _gridLogs = new();
    private TextBox _txtSearch = new() { Width = 250 };
    private ComboBox _cmbSeverity = new() { Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
    private ComboBox _cmbDownloadSource = new() { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };

    public LogCenterUserControl()
        : this(() => string.Empty, () => 22, () => string.Empty, () => string.Empty, () => string.Empty)
    {
    }

    public LogCenterUserControl(
        Func<string> hostProvider,
        Func<int> sshPortProvider,
        Func<string> serverUserProvider,
        Func<string> sshKeyPathProvider,
        Func<string> sshPasswordProvider)
    {
        _hostProvider = hostProvider;
        _sshPortProvider = sshPortProvider;
        _serverUserProvider = serverUserProvider;
        _sshKeyPathProvider = sshKeyPathProvider;
        _sshPasswordProvider = sshPasswordProvider;

        BackColor = Color.FromArgb(10, 14, 22);
        ForeColor = Color.FromArgb(220, 230, 240);

        _gridLogs.Dock = DockStyle.Fill;
        UIHelper.AplicarEstiloModernoTabla(_gridLogs);

        var pnlTop = new Panel { Dock = DockStyle.Top, Height = 110, BackColor = Color.FromArgb(22, 32, 46) };
        _cmbSeverity.Items.AddRange(["All", "Info", "Warning", "Error", "Success"]);
        _cmbSeverity.SelectedIndex = 0;
        _cmbSeverity.BackColor = Color.FromArgb(18, 28, 40);
        _cmbSeverity.ForeColor = Color.FromArgb(245, 248, 251);
        _cmbSeverity.FlatStyle = FlatStyle.Flat;
        _cmbSeverity.DropDownStyle = ComboBoxStyle.DropDownList;

        _txtSearch.PlaceholderText = "Buscar en logs...";
        _txtSearch.BackColor = Color.FromArgb(18, 28, 40);
        _txtSearch.ForeColor = Color.FromArgb(245, 248, 251);
        _txtSearch.BorderStyle = BorderStyle.FixedSingle;
        _txtSearch.Width = 310;

        _cmbDownloadSource.Items.AddRange([
            "migrator-local - Migrator local",
            "migrator - Migrator remoto (VPS)",
            "holos-api - Docker API",
            "holos-front - Docker Front",
            "holos-sql - Docker SQL",
            "host-syslog - Log del sistema VPS",
            "host-auth - Log de autenticación SSH"
        ]);
        _cmbDownloadSource.SelectedIndex = 0;
        _cmbDownloadSource.BackColor = Color.FromArgb(18, 28, 40);
        _cmbDownloadSource.ForeColor = Color.FromArgb(245, 248, 251);
        _cmbDownloadSource.FlatStyle = FlatStyle.Flat;
        _cmbDownloadSource.DropDownStyle = ComboBoxStyle.DropDownList;

        var btnRef = new Button
        {
            Text = "Actualizar",
            FlatStyle = FlatStyle.Flat,
            Width = 130,
            Height = 40,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(14, 110, 198)
        };
        btnRef.FlatAppearance.BorderSize = 0;
        btnRef.Click += (_, _) => RefreshLogs();

        var btnExp = new Button
        {
            Text = "Exportar",
            FlatStyle = FlatStyle.Flat,
            Width = 130,
            Height = 40,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(20, 40, 62)
        };
        btnExp.FlatAppearance.BorderColor = Color.FromArgb(73, 102, 136);
        btnExp.FlatAppearance.BorderSize = 1;
        btnExp.Click += (_, _) => ExportLogs();

        var btnDownload = new Button
        {
            Text = "Descargar",
            FlatStyle = FlatStyle.Flat,
            Width = 150,
            Height = 42,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(26, 62, 95)
        };
        btnDownload.FlatAppearance.BorderColor = Color.FromArgb(81, 149, 205);
        btnDownload.FlatAppearance.BorderSize = 1;
        btnDownload.Click += async (_, _) => await DownloadSelectedLogAsync();

        _cmbSeverity.SelectedIndexChanged += (_, _) => RefreshLogs();
        _txtSearch.TextChanged += (_, _) => RefreshLogs();

        _cmbSeverity.Location = new Point(12, 20);
        _txtSearch.Location = new Point(178, 20);
        btnRef.Location = new Point(500, 18);
        btnExp.Location = new Point(642, 18);

        var lblDownload = new Label
        {
            AutoSize = true,
            ForeColor = Color.FromArgb(208, 228, 248),
            Font = new Font("Segoe UI", 10F, FontStyle.Regular),
            Text = "Log:",
            Location = new Point(10, 64)
        };
        _cmbDownloadSource.Location = new Point(116, 62);
        btnDownload.Location = new Point(380, 60);

        pnlTop.Controls.AddRange([_cmbSeverity, _txtSearch, btnRef, btnExp, lblDownload, _cmbDownloadSource, btnDownload]);

        Controls.Add(_gridLogs);
        Controls.Add(pnlTop);
        Controls.Add(new Label { Text = "LOG CENTER", Dock = DockStyle.Top, Font = new Font("Consolas", 14, FontStyle.Bold), Height = 40, ForeColor = Color.Cyan });

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            RefreshLogs();
        }
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

    private async Task DownloadSelectedLogAsync()
    {
        var selected = _cmbDownloadSource.SelectedItem?.ToString() ?? "migrator-local";
        var sourceCode = ParseOptionCode(selected, "migrator-local");

        string? content;
        if (string.Equals(sourceCode, "migrator-local", StringComparison.OrdinalIgnoreCase))
        {
            var localPath = GetLocalMigratorLogPath();
            if (!File.Exists(localPath))
            {
                MessageBox.Show("No existe el archivo migrator_logs.txt local.", "Log no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            content = await File.ReadAllTextAsync(localPath, Encoding.UTF8);
        }
        else
        {
            var host = _hostProvider().Trim();
            var user = _serverUserProvider().Trim();
            var keyPath = _sshKeyPathProvider().Trim();
            var password = _sshPasswordProvider().Trim();
            var port = _sshPortProvider();

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user))
            {
                MessageBox.Show("Configura SERVER_HOST y SERVER_USER para descargar logs remotos.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(keyPath) && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Se requiere llave SSH o password SSH para logs remotos.", "Credenciales SSH", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            content = await HealthCheckService.TryGetRemoteLogAsync(
                host,
                user,
                port,
                string.IsNullOrWhiteSpace(keyPath) ? null : keyPath,
                string.IsNullOrWhiteSpace(password) ? null : password,
                sourceCode,
                500,
                14000);

            if (string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("No se pudo obtener el log remoto. Revisa conectividad SSH y permisos.", "Descarga fallida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        using var fd = new SaveFileDialog
        {
            Filter = "LOG|*.log|TXT|*.txt",
            FileName = $"{sourceCode}-{DateTime.Now:yyyyMMdd-HHmmss}.log"
        };

        if (fd.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(fd.FileName, content, Encoding.UTF8);
            MessageBox.Show("Log descargado correctamente.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
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
            MessageBox.Show("Exportado", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private static string ParseOptionCode(string? value, string fallback)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return fallback;
        }

        var idx = value.IndexOf(" - ", StringComparison.Ordinal);
        if (idx > 0)
        {
            return value[..idx].Trim();
        }

        return value.Trim();
    }

    private void InitializeComponent()
    {

    }

    private static string GetLocalMigratorLogPath()
    {
        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "migrator_logs.txt"));
    }
}

public class SettingsUserControl : UserControl
{
    private readonly Func<string> _hostProvider;
    private readonly Func<int> _sshPortProvider;
    private readonly Func<string> _serverUserProvider;
    private readonly Func<string> _sshKeyPathProvider;
    private readonly Func<string> _sshPasswordProvider;
    private readonly Func<string> _remoteRepoPathProvider;
    private DataGridView _gridEnv = new();

    private readonly TextBox _txtPgDb = new() { Text = "HolosCoreOpsPostgres" };
    private readonly TextBox _txtPgUser = new() { Text = "holoscoreops" };
    private readonly TextBox _txtPgPassword = new() { PasswordChar = '●' };
    private readonly TextBox _txtAppServerRoot = new();
    private readonly TextBox _txtAppClientRoot = new();
    private readonly TextBox _txtAppCorsOrigins = new();
    private readonly TextBox _txtJwtKey = new() { PasswordChar = '●' };
    private readonly CheckBox _chkShowSecrets = new() { Text = "Mostrar secretos", AutoSize = true };
    private readonly Label _lblDeployEnvStatus = new() { AutoSize = true };

    public SettingsUserControl()
        : this(() => string.Empty, () => 22, () => string.Empty, () => string.Empty, () => string.Empty, () => "/root/OmniSuite")
    {
    }

    public SettingsUserControl(
        Func<string> hostProvider,
        Func<int> sshPortProvider,
        Func<string> serverUserProvider,
        Func<string> sshKeyPathProvider,
        Func<string> sshPasswordProvider,
        Func<string>? remoteRepoPathProvider = null)
    {
        _hostProvider = hostProvider;
        _sshPortProvider = sshPortProvider;
        _serverUserProvider = serverUserProvider;
        _sshKeyPathProvider = sshKeyPathProvider;
        _sshPasswordProvider = sshPasswordProvider;
        _remoteRepoPathProvider = remoteRepoPathProvider ?? (() => "/root/OmniSuite");

        BackColor = Color.FromArgb(9, 9, 11);
        ForeColor = Color.Cyan;

        _gridEnv.Dock = DockStyle.Fill;
        UIHelper.AplicarEstiloModernoTabla(_gridEnv);

        var btnRefreshEnv = new Button
        {
            Text = "Cargar variables del VPS",
            FlatStyle = FlatStyle.Flat,
            Width = 250,
            Height = 36,
            ForeColor = Color.FromArgb(150, 237, 255),
            BackColor = Color.FromArgb(19, 47, 71)
        };
        btnRefreshEnv.FlatAppearance.BorderColor = Color.FromArgb(54, 121, 172);
        btnRefreshEnv.Click += async (_, _) => await RefreshEnvironmentVariablesAsync();

        var btnExportEnv = new Button
        {
            Text = "Exportar variables",
            FlatStyle = FlatStyle.Flat,
            Width = 170,
            Height = 36,
            ForeColor = Color.FromArgb(173, 220, 255),
            BackColor = Color.FromArgb(24, 36, 52)
        };
        btnExportEnv.FlatAppearance.BorderColor = Color.FromArgb(43, 58, 76);
        btnExportEnv.Click += (_, _) => ExportEnvironmentVariables();

        var infoText = new Label
        {
            Text = "Pantalla enfocada solo en variables de entorno del VPS (host y contenedores).",
            Dock = DockStyle.Top,
            Height = 32,
            ForeColor = Color.FromArgb(184, 206, 229),
            Font = new Font("Segoe UI", 9.5F),
            AutoEllipsis = true,
            Padding = new Padding(20, 0, 20, 0),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 44,
            AutoSize = false,
            WrapContents = false,
            BackColor = Color.Transparent
        };
        actions.Controls.Add(btnRefreshEnv);
        actions.Controls.Add(btnExportEnv);

        var envPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 8, 20, 20) };
        envPanel.Controls.Add(_gridEnv);
        envPanel.Controls.Add(actions);

        Controls.Add(envPanel);
        Controls.Add(infoText);
        Controls.Add(new Label { Text = "VARIABLES DE ENTORNO VPS", Dock = DockStyle.Top, Font = new Font("Consolas", 14, FontStyle.Bold), Height = 40, ForeColor = Color.Cyan });
        Controls.Add(BuildDeployEnvSection());

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime
            && !string.IsNullOrWhiteSpace(_hostProvider())
            && !string.IsNullOrWhiteSpace(_serverUserProvider())
            && (!string.IsNullOrWhiteSpace(_sshKeyPathProvider()) || !string.IsNullOrWhiteSpace(_sshPasswordProvider())))
        {
            _ = RefreshEnvironmentVariablesAsync();
        }
    }

    private async Task RefreshEnvironmentVariablesAsync()
    {
        var host = _hostProvider().Trim();
        var user = _serverUserProvider().Trim();
        var keyPath = _sshKeyPathProvider().Trim();
        var password = _sshPasswordProvider().Trim();
        var port = _sshPortProvider();

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user))
        {
            MessageBox.Show("Configura SERVER_HOST y SERVER_USER para consultar variables remotas.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(keyPath) && string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Se requiere llave SSH o password SSH para consultar variables remotas.", "Credenciales SSH", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var output = await HealthCheckService.TryGetRemoteEnvironmentSnapshotAsync(
            host,
            user,
            port,
            string.IsNullOrWhiteSpace(keyPath) ? null : keyPath,
            string.IsNullOrWhiteSpace(password) ? null : password,
            14000);

        if (string.IsNullOrWhiteSpace(output))
        {
            MessageBox.Show("No se pudieron obtener variables del VPS.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var table = new DataTable();
        table.Columns.Add("Origen");
        table.Columns.Add("Variable");
        table.Columns.Add("Valor");

        var origin = "HOST";
        foreach (var raw in output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var line = raw.Trim();
            if (line.StartsWith("[HOST]", StringComparison.OrdinalIgnoreCase))
            {
                origin = "HOST";
                continue;
            }

            if (line.StartsWith("[CONTAINER:", StringComparison.OrdinalIgnoreCase) && line.EndsWith(']'))
            {
                origin = line[11..^1];
                continue;
            }

            var sep = line.IndexOf('=');
            if (sep <= 0)
            {
                continue;
            }

            table.Rows.Add(origin, line[..sep], line[(sep + 1)..]);
        }

        _gridEnv.DataSource = table;
    }

    private void ExportEnvironmentVariables()
    {
        if (_gridEnv.DataSource is not DataTable table || table.Rows.Count == 0)
        {
            MessageBox.Show("No hay variables para exportar.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var fd = new SaveFileDialog
        {
            Filter = "TXT|*.txt",
            FileName = $"vps-env-{DateTime.Now:yyyyMMdd-HHmmss}.txt"
        };

        if (fd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        var sb = new StringBuilder();
        foreach (DataRow row in table.Rows)
        {
            sb.AppendLine($"[{row[0]}] {row[1]}={row[2]}");
        }

        File.WriteAllText(fd.FileName, sb.ToString(), Encoding.UTF8);
        MessageBox.Show("Variables exportadas.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private string RemoteEnvPath => _remoteRepoPathProvider().Trim().TrimEnd('/') + "/.env";

    private Panel BuildDeployEnvSection()
    {
        var root = new Panel { Dock = DockStyle.Top, Height = 372, Padding = new Padding(20, 10, 20, 10) };

        var title = new Label
        {
            Text = "VARIABLES DE ENTORNO DE PRODUCCIÓN (.env remoto)",
            Dock = DockStyle.Top,
            Height = 30,
            Font = new Font("Consolas", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(150, 237, 255)
        };

        _lblDeployEnvStatus.Text = "Estado desconocido — pulsa \"Leer .env actual del servidor\".";
        _lblDeployEnvStatus.ForeColor = Color.FromArgb(184, 206, 229);
        _lblDeployEnvStatus.Font = new Font("Consolas", 9F);

        _chkShowSecrets.ForeColor = Color.FromArgb(184, 206, 229);
        _chkShowSecrets.CheckedChanged += (_, _) =>
        {
            var reveal = _chkShowSecrets.Checked;
            _txtPgPassword.PasswordChar = reveal ? '\0' : '●';
            _txtJwtKey.PasswordChar = reveal ? '\0' : '●';
        };

        var statusRow = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 26, WrapContents = false };
        statusRow.Controls.Add(_lblDeployEnvStatus);

        var showRow = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 24, WrapContents = false };
        showRow.Controls.Add(_chkShowSecrets);

        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 246,
            ColumnCount = 3,
            RowCount = 7
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        for (var i = 0; i < 7; i++)
        {
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        }

        var btnGenPgPassword = CreateSmallButton("Generar");
        btnGenPgPassword.Click += (_, _) => _txtPgPassword.Text = SecretGenerator.Generate(24);

        var btnGenJwt = CreateSmallButton("Generar");
        btnGenJwt.Click += (_, _) => _txtJwtKey.Text = SecretGenerator.Generate(64);

        var btnUseIpServer = CreateSmallButton("usar IP actual");
        btnUseIpServer.Click += (_, _) => _txtAppServerRoot.Text = $"http://{_hostProvider().Trim()}/";

        var btnUseIpClient = CreateSmallButton("usar IP actual");
        btnUseIpClient.Click += (_, _) => _txtAppClientRoot.Text = $"http://{_hostProvider().Trim()}/";

        var btnUseIpCors = CreateSmallButton("usar IP actual");
        btnUseIpCors.Click += (_, _) => _txtAppCorsOrigins.Text = $"http://{_hostProvider().Trim()}";

        AddFieldRow(table, 0, "POSTGRES_DB", _txtPgDb, null);
        AddFieldRow(table, 1, "POSTGRES_USER", _txtPgUser, null);
        AddFieldRow(table, 2, "POSTGRES_PASSWORD", _txtPgPassword, btnGenPgPassword);
        AddFieldRow(table, 3, "APP_SERVER_ROOT", _txtAppServerRoot, btnUseIpServer);
        AddFieldRow(table, 4, "APP_CLIENT_ROOT", _txtAppClientRoot, btnUseIpClient);
        AddFieldRow(table, 5, "APP_CORS_ORIGINS", _txtAppCorsOrigins, btnUseIpCors);
        AddFieldRow(table, 6, "JWT_SECURITY_KEY", _txtJwtKey, btnGenJwt);

        var btnRead = new Button
        {
            Text = "Leer .env actual del servidor",
            FlatStyle = FlatStyle.Flat,
            Width = 230,
            Height = 36,
            ForeColor = Color.FromArgb(173, 220, 255),
            BackColor = Color.FromArgb(24, 36, 52)
        };
        btnRead.FlatAppearance.BorderColor = Color.FromArgb(43, 58, 76);
        btnRead.Click += async (_, _) => await LoadRemoteEnvAsync();

        var btnWrite = new Button
        {
            Text = "Escribir .env al servidor",
            FlatStyle = FlatStyle.Flat,
            Width = 210,
            Height = 36,
            ForeColor = Color.FromArgb(10, 20, 14),
            BackColor = Color.FromArgb(34, 255, 102),
            Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold)
        };
        btnWrite.FlatAppearance.BorderColor = Color.FromArgb(34, 255, 102);
        btnWrite.Click += async (_, _) => await WriteRemoteEnvAsync();

        var footerRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 44,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false
        };
        footerRow.Controls.Add(btnWrite);
        footerRow.Controls.Add(btnRead);

        root.Controls.Add(footerRow);
        root.Controls.Add(table);
        root.Controls.Add(showRow);
        root.Controls.Add(statusRow);
        root.Controls.Add(title);

        return root;
    }

    private static Button CreateSmallButton(string text) => new()
    {
        Text = text,
        FlatStyle = FlatStyle.Flat,
        Width = 150,
        Height = 26,
        Font = new Font("Consolas", 8.5F),
        ForeColor = Color.FromArgb(150, 237, 255),
        BackColor = Color.FromArgb(19, 47, 71),
        FlatAppearance = { BorderColor = Color.FromArgb(54, 121, 172) }
    };

    private static void AddFieldRow(TableLayoutPanel table, int row, string labelText, TextBox textBox, Button? actionButton)
    {
        var label = new Label
        {
            Text = labelText,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Consolas", 9F),
            ForeColor = Color.FromArgb(150, 200, 220)
        };
        textBox.Dock = DockStyle.Fill;
        textBox.Font = new Font("Consolas", 9.5F);
        textBox.BackColor = Color.FromArgb(18, 28, 40);
        textBox.ForeColor = Color.FromArgb(245, 248, 251);
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.Margin = new Padding(0, 4, 8, 4);

        table.Controls.Add(label, 0, row);
        table.Controls.Add(textBox, 1, row);
        if (actionButton != null)
        {
            actionButton.Margin = new Padding(0, 3, 0, 3);
            actionButton.Dock = DockStyle.Left;
            table.Controls.Add(actionButton, 2, row);
        }
    }

    private async Task LoadRemoteEnvAsync()
    {
        var host = _hostProvider().Trim();
        var user = _serverUserProvider().Trim();
        var keyPath = _sshKeyPathProvider().Trim();
        var password = _sshPasswordProvider().Trim();
        var port = _sshPortProvider();

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user))
        {
            MessageBox.Show("Configura Servidor y Usuario para leer el .env remoto.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _lblDeployEnvStatus.Text = "Leyendo .env remoto...";
        _lblDeployEnvStatus.ForeColor = Color.FromArgb(184, 206, 229);

        var path = RemoteEnvPath;
        var content = await HealthCheckService.TryReadRemoteFileAsync(
            host, user, port,
            string.IsNullOrWhiteSpace(keyPath) ? null : keyPath,
            string.IsNullOrWhiteSpace(password) ? null : password,
            path);

        if (content == null)
        {
            _lblDeployEnvStatus.Text = $"⚠ No se pudo conectar al servidor para leer {path}.";
            _lblDeployEnvStatus.ForeColor = Color.FromArgb(255, 120, 120);
            return;
        }

        if (HealthCheckService.IsRemoteFileNotFound(content))
        {
            _lblDeployEnvStatus.Text = $"⚠ No existe {path} todavía — se creará al escribir.";
            _lblDeployEnvStatus.ForeColor = Color.FromArgb(245, 166, 35);
            return;
        }

        var values = ParseEnvContent(content);
        SetIfPresent(values, "POSTGRES_DB", _txtPgDb);
        SetIfPresent(values, "POSTGRES_USER", _txtPgUser);
        SetIfPresent(values, "POSTGRES_PASSWORD", _txtPgPassword);
        SetIfPresent(values, "APP_SERVER_ROOT", _txtAppServerRoot);
        SetIfPresent(values, "APP_CLIENT_ROOT", _txtAppClientRoot);
        SetIfPresent(values, "APP_CORS_ORIGINS", _txtAppCorsOrigins);
        SetIfPresent(values, "JWT_SECURITY_KEY", _txtJwtKey);

        _lblDeployEnvStatus.Text = $"✓ Cargado desde {path}.";
        _lblDeployEnvStatus.ForeColor = Color.FromArgb(120, 255, 160);
    }

    private async Task WriteRemoteEnvAsync()
    {
        var host = _hostProvider().Trim();
        var user = _serverUserProvider().Trim();
        var keyPath = _sshKeyPathProvider().Trim();
        var password = _sshPasswordProvider().Trim();
        var port = _sshPortProvider();
        var path = RemoteEnvPath;

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user))
        {
            MessageBox.Show("Configura Servidor y Usuario para escribir el .env remoto.", "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(_txtPgDb.Text)) missing.Add("POSTGRES_DB");
        if (string.IsNullOrWhiteSpace(_txtPgUser.Text)) missing.Add("POSTGRES_USER");
        if (string.IsNullOrWhiteSpace(_txtPgPassword.Text)) missing.Add("POSTGRES_PASSWORD");
        if (string.IsNullOrWhiteSpace(_txtAppServerRoot.Text)) missing.Add("APP_SERVER_ROOT");
        if (string.IsNullOrWhiteSpace(_txtAppClientRoot.Text)) missing.Add("APP_CLIENT_ROOT");
        if (string.IsNullOrWhiteSpace(_txtAppCorsOrigins.Text)) missing.Add("APP_CORS_ORIGINS");
        if (string.IsNullOrWhiteSpace(_txtJwtKey.Text)) missing.Add("JWT_SECURITY_KEY");

        if (missing.Count > 0)
        {
            MessageBox.Show($"Faltan valores: {string.Join(", ", missing)}", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"Esto sobreescribe {path} en {host}. ¿Continuar?",
            "Confirmar escritura remota",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        _lblDeployEnvStatus.Text = "Escribiendo .env remoto...";
        _lblDeployEnvStatus.ForeColor = Color.FromArgb(184, 206, 229);

        var content = BuildEnvContent();
        var ok = await HealthCheckService.WriteRemoteFileAsync(
            host, user, port,
            string.IsNullOrWhiteSpace(keyPath) ? null : keyPath,
            string.IsNullOrWhiteSpace(password) ? null : password,
            path,
            content);

        if (ok)
        {
            _lblDeployEnvStatus.Text = $"✓ Escrito en {path}.";
            _lblDeployEnvStatus.ForeColor = Color.FromArgb(120, 255, 160);
        }
        else
        {
            _lblDeployEnvStatus.Text = $"⚠ Fallo al escribir {path}.";
            _lblDeployEnvStatus.ForeColor = Color.FromArgb(255, 120, 120);
            MessageBox.Show("No se pudo escribir el archivo remoto. Revisa la conexión SSH.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string BuildEnvContent()
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Generado por HolosMigratorUI — Settings > Variables de entorno (Producción)");
        sb.AppendLine($"# {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine();
        sb.AppendLine($"POSTGRES_DB={_txtPgDb.Text.Trim()}");
        sb.AppendLine($"POSTGRES_USER={_txtPgUser.Text.Trim()}");
        sb.AppendLine($"POSTGRES_PASSWORD={_txtPgPassword.Text.Trim()}");
        sb.AppendLine();
        sb.AppendLine($"APP_SERVER_ROOT={_txtAppServerRoot.Text.Trim()}");
        sb.AppendLine($"APP_CLIENT_ROOT={_txtAppClientRoot.Text.Trim()}");
        sb.AppendLine($"APP_CORS_ORIGINS={_txtAppCorsOrigins.Text.Trim()}");
        sb.AppendLine();
        sb.AppendLine($"JWT_SECURITY_KEY={_txtJwtKey.Text.Trim()}");
        return sb.ToString();
    }

    private static void SetIfPresent(IReadOnlyDictionary<string, string> values, string key, TextBox target)
    {
        if (values.TryGetValue(key, out var value))
        {
            target.Text = value;
        }
    }

    private static Dictionary<string, string> ParseEnvContent(string content)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var raw in content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var line = raw.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var sep = line.IndexOf('=');
            if (sep <= 0)
            {
                continue;
            }

            result[line[..sep].Trim()] = line[(sep + 1)..].Trim();
        }

        return result;
    }
}

public class AlertsUserControl : UserControl
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private DataGridView _grid = new();
    private Label _lblSuccess7d = new();
    private Label _lblSuccess30d = new();
    private Label _lblMttr = new();
    private Label _lblAvgDuration = new();

    public AlertsUserControl()
    {
        BackColor = Color.FromArgb(9, 9, 11);
        _grid.Dock = DockStyle.Fill;
        UIHelper.AplicarEstiloModernoTabla(_grid);

        var pnlTop = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.FromArgb(10, 16, 27) };
        var btnRef = new Button
        {
            Text = "Refrescar Alertas",
            FlatStyle = FlatStyle.Flat,
            Width = 220,
            Height = 40,
            Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(0, 255, 255),
            BackColor = Color.FromArgb(14, 40, 52),
            Location = new Point(10, 16)
        };
        btnRef.FlatAppearance.BorderColor = Color.FromArgb(0, 188, 221);
        btnRef.Click += (_, _) => RefreshAlerts();
        pnlTop.Controls.Add(btnRef);

        var pnlKpis = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 108,
            ColumnCount = 4,
            Padding = new Padding(8, 8, 8, 8),
            BackColor = Color.FromArgb(9, 13, 20)
        };
        pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

        pnlKpis.Controls.Add(BuildKpiCard("Exito 7 dias", _lblSuccess7d, Color.FromArgb(90, 230, 165)), 0, 0);
        pnlKpis.Controls.Add(BuildKpiCard("Exito 30 dias", _lblSuccess30d, Color.FromArgb(110, 208, 255)), 1, 0);
        pnlKpis.Controls.Add(BuildKpiCard("MTTR", _lblMttr, Color.FromArgb(255, 209, 102)), 2, 0);
        pnlKpis.Controls.Add(BuildKpiCard("Duracion promedio", _lblAvgDuration, Color.FromArgb(197, 166, 255)), 3, 0);

        Controls.Add(_grid);
        Controls.Add(pnlKpis);
        Controls.Add(pnlTop);
        Controls.Add(new Label { Text = "ALERTAS Y KPIs", Dock = DockStyle.Top, Font = new Font("Consolas", 14, FontStyle.Bold), Height = 40, ForeColor = Color.Cyan });

        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            RefreshAlerts();
        }
    }

    public void RefreshAlerts()
    {
        var table = new DataTable();
        table.Columns.Add("Fecha"); table.Columns.Add("Regla"); table.Columns.Add("Severidad"); table.Columns.Add("Mensaje");
        foreach (var a in _state.GetAlerts(false).OrderByDescending(x => x.CreatedAt))
            table.Rows.Add(a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), a.Rule, a.Severity, a.Message);

        _grid.DataSource = table;

        var runs = _state.GetRuns();
        var hasData = runs.Count > 0;
        var (successRate7d, successRate30d, mttrMinutes, avgDurationMinutes) = _state.GetKpis();

        _lblSuccess7d.Text = hasData ? $"{successRate7d:F1}%" : "--";
        _lblSuccess30d.Text = hasData ? $"{successRate30d:F1}%" : "--";
        _lblMttr.Text = hasData && mttrMinutes > 0 ? $"{mttrMinutes:F1} min" : "--";
        _lblAvgDuration.Text = hasData && avgDurationMinutes > 0 ? $"{avgDurationMinutes:F1} min" : "--";
    }

    private static Panel BuildKpiCard(string title, Label valueLabel, Color valueColor)
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(6),
            BackColor = Color.FromArgb(20, 30, 45),
            Padding = new Padding(12, 10, 12, 8)
        };

        var titleLabel = new Label
        {
            Dock = DockStyle.Top,
            Height = 26,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(172, 197, 224),
            Text = title
        };

        valueLabel.Dock = DockStyle.Fill;
        valueLabel.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
        valueLabel.ForeColor = valueColor;
        valueLabel.TextAlign = ContentAlignment.MiddleLeft;
        valueLabel.Text = "--";

        card.Controls.Add(valueLabel);
        card.Controls.Add(titleLabel);
        return card;
    }
}

