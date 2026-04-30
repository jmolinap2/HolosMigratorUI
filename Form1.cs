using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HolosMigratorUI;

public partial class Form1 : Form
{
    private Process? _runningProcess;
    private string SettingsFilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "HolosMigratorUI",
        "settings.json");

    public Form1()
    {
        InitializeComponent();
        FixDesignerBugs();
        BindEvents();
        LoadSettings();
        LoadEnvVariables();
        ApplyUiState();
        ConfigureCyberpunkUI();
        FormClosing += (_, _) => SaveSettings();
        AppendLog("✅ Sistema listo. Ingresa tu Password SSH y presiona '▶ EJECUTAR' arriba a la derecha.");
    }

    private void FixDesignerBugs()
    {
        // Forzar visibilidad y tamaño de textboxes rotos por el diseñador de WinForms
        _txtSshPassword.Multiline = false;
        _txtSshPassword.Size = new Size(_panelSshPassword.Width - 40, 25);
        _txtSshPassword.Location = new Point(5, 5);

        // Seleccionar el primer elemento en todos los ComboBoxes para que no aparezcan vacíos
        if (_cmbAction.Items.Count > 0 && _cmbAction.SelectedIndex == -1) _cmbAction.SelectedIndex = 0;
        if (_cmbSshAuth.Items.Count > 0 && _cmbSshAuth.SelectedIndex == -1) _cmbSshAuth.SelectedIndex = 0;
        if (_cmbDeployTarget.Items.Count > 0 && _cmbDeployTarget.SelectedIndex == -1) _cmbDeployTarget.SelectedIndex = 0;
        if (_cmbMigrationMode.Items.Count > 0 && _cmbMigrationMode.SelectedIndex == -1) _cmbMigrationMode.SelectedIndex = 0;
    }

    private void LoadEnvVariables()
    {
        try
        {
            var envPath = FindEnvFile();
            if (envPath == null)
            {
                AppendLog("⚠ .env no encontrado. Crea un archivo .env en la raíz del proyecto.");
                return;
            }

            DotNetEnv.Env.Load(envPath);
            AppendLog($"✓ .env cargado desde: {envPath}");

            var host = DotNetEnv.Env.GetString("SERVER_HOST");
            if (!string.IsNullOrEmpty(host)) _txtServerHost.Text = host;

            var user = DotNetEnv.Env.GetString("SERVER_USER");
            if (!string.IsNullOrEmpty(user)) _txtServerUser.Text = user;

            var port = DotNetEnv.Env.GetInt("SSH_PORT", 0);
            if (port > 0) _numSshPort.Value = port;

            var pass = DotNetEnv.Env.GetString("SSH_PASSWORD");
            if (!string.IsNullOrEmpty(pass)) _txtSshPassword.Text = pass;

            var repo = DotNetEnv.Env.GetString("REPO_LOCAL");
            if (!string.IsNullOrEmpty(repo)) _txtRepoPath.Text = repo;

            var key = DotNetEnv.Env.GetString("SSH_KEY_PATH");
            if (!string.IsNullOrEmpty(key)) _txtSshKeyPath.Text = key;

            var gitToken = DotNetEnv.Env.GetString("GIT_TOKEN");
            if (!string.IsNullOrEmpty(gitToken)) _txtGitToken.Text = gitToken;
        }
        catch (Exception ex)
        {
            AppendLog($"✗ Error cargando .env: {ex.Message}");
        }
    }

    private static string? FindEnvFile()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, ".env");
            if (File.Exists(candidate)) return candidate;
            dir = dir.Parent;
        }
        return null;
    }

    private void BindEvents()
    {
        _cmbAction.SelectedIndexChanged += (_, _) => ApplyUiState();
        _cmbSshAuth.SelectedIndexChanged += (_, _) => ApplyUiState();
        _txtSshPassword.TextChanged += (_, _) => ApplyUiState();
        _cmbMigrationMode.SelectedIndexChanged += (_, _) => ApplyUiState();
        _chkSkipMigrations.CheckedChanged += (_, _) => ApplyUiState();
        _btnRun.Click += async (_, _) => await RunSelectedActionAsync();
        _btnStop.Click += (_, _) => StopCurrentProcess();
        _btnOpenScripts.Click += (_, _) => OpenScriptsFolder();
        _btnOpenLog.Click += (_, _) => OpenLogFile();
        _btnShowPassword.Click += (_, _) =>
        {
            _txtSshPassword.UseSystemPasswordChar = !_txtSshPassword.UseSystemPasswordChar;
            _btnShowPassword.Text = _txtSshPassword.UseSystemPasswordChar ? "👁" : "🙈";
        };
    }

    private void ApplyUiState()
    {
        var action = GetSelectedAction();
        var sshAuth = GetSelectedSshAuth();
        var effectiveSshAuth = GetEffectiveSshAuthMode();
        var migrationMode = GetSelectedMigrationMode();
        var skippingMigrations = action == "Deploy completo" && _chkSkipMigrations.Checked;

        _cmbDeployTarget.Enabled = action == "Deploy completo";
        _cmbMigrationMode.Enabled = !skippingMigrations;
        
        bool tenantActive = migrationMode == "U" && !skippingMigrations;
        _txtTenant.Enabled = tenantActive;
        _txtTenant.PlaceholderText = tenantActive ? "Escribe el nombre del tenant..." : "(Habilitado solo si modo es 'U')";
        _txtTenant.BackColor = tenantActive ? Color.FromArgb(9, 9, 11) : Color.FromArgb(20, 20, 25);

        _chkSkipPull.Enabled = action == "Deploy completo";
        _chkSkipMigrations.Enabled = action == "Deploy completo";
        _chkSkipBuild.Enabled = action == "Deploy completo";
        _chkSkipPublicChecks.Enabled = action == "Deploy completo";

        _txtSshKeyPath.Enabled = sshAuth != "Password";
        _chkSshBatchMode.Enabled = sshAuth != "Password";
        _txtSshPassword.Enabled = sshAuth != "Key";
        _chkRememberSshPassword.Enabled = sshAuth != "Key";
        _chkInteractiveWindowForPassword.Enabled =
            effectiveSshAuth == "Password" && string.IsNullOrWhiteSpace(_txtSshPassword.Text);

        if (effectiveSshAuth != "Password")
        {
            _chkInteractiveWindowForPassword.Checked = false;
        }
    }

    private async Task RunSelectedActionAsync()
    {
        if (_runningProcess != null)
        {
            AppendLog("Ya hay un proceso en ejecución.");
            return;
        }

        try
        {
            var scriptPath = GetScriptPath();
            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException($"No se encontró el script: {scriptPath}");
            }

            var args = BuildPowerShellArgs(scriptPath);
            var action = GetSelectedAction();
            var sshAuth = GetSelectedSshAuth();
            var effectiveSshAuth = GetEffectiveSshAuthMode();
            var hasPassword = !string.IsNullOrWhiteSpace(_txtSshPassword.Text);

            SaveSettings();

            AppendLog($"Acción: {action}");
            AppendLog($"Script: {scriptPath}");
            AppendLog($"Auth SSH efectivo: {effectiveSshAuth} (seleccionado: {sshAuth})");

            if (effectiveSshAuth == "Password" && hasPassword)
            {
                AppendLog("Usando contraseña SSH almacenada en UI para ejecución automática.");
            }

            if (effectiveSshAuth == "Password" && !hasPassword && _chkInteractiveWindowForPassword.Checked)
            {
                StartInteractiveWindow(args);
                AppendLog("Se abrió una terminal interactiva para ingresar la contraseña SSH.");
                return;
            }

            await RunCapturedProcessAsync(args);
        }
        catch (Exception ex)
        {
            AppendLog("ERROR: " + ex.Message);
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string GetScriptPath()
    {
        var repoPath = _txtRepoPath.Text.Trim();
        if (string.IsNullOrWhiteSpace(repoPath))
        {
            throw new InvalidOperationException("Debes indicar Repo local.");
        }

        var scriptFile = GetSelectedAction() == "Deploy completo"
            ? "deploy-hostinger.ps1"
            : "migrate-hostinger-ui.ps1";

        return Path.Combine(repoPath, "scripts", scriptFile);
    }

    private List<string> BuildPowerShellArgs(string scriptPath)
    {
        ValidateInputs();
        var effectiveSshAuth = GetEffectiveSshAuthMode();

        var args = new List<string>
        {
            "-NoProfile",
            "-ExecutionPolicy", "Bypass",
            "-File", scriptPath,
            "-ServerHost", _txtServerHost.Text.Trim(),
            "-ServerUser", _txtServerUser.Text.Trim(),
            "-SshPort", _numSshPort.Value.ToString(),
            "-SshAuthMode", effectiveSshAuth,
            "-RemoteRepoPath", _txtRemoteRepoPath.Text.Trim(),
            "-ComposeFile", _txtComposeFile.Text.Trim(),
            "-MigrationMode", GetSelectedMigrationMode()
        };

        var keyPath = _txtSshKeyPath.Text.Trim();
        if (!string.IsNullOrWhiteSpace(keyPath))
        {
            args.Add("-SshKeyPath");
            args.Add(keyPath);
        }

        if (_chkSshBatchMode.Checked)
        {
            args.Add("-SshBatchMode");
        }

        var sshPassword = _txtSshPassword.Text;
        if (effectiveSshAuth == "Password" && !string.IsNullOrWhiteSpace(sshPassword))
        {
            args.Add("-SshPassword");
            args.Add(sshPassword);
        }

        if (GetSelectedMigrationMode() == "U")
        {
            args.Add("-TenantIdentifier");
            args.Add(_txtTenant.Text.Trim());
        }

        if (GetSelectedAction() == "Deploy completo")
        {
            args.Add("-Branch");
            args.Add(_txtBranch.Text.Trim());

            args.Add("-DeployTarget");
            args.Add(GetSelectedDeployTarget());

            var gitToken = _txtGitToken.Text.Trim();
            if (!string.IsNullOrWhiteSpace(gitToken))
            {
                args.Add("-GitToken");
                args.Add(gitToken);
            }

            if (_chkSkipPull.Checked) { args.Add("-SkipPull"); }
            if (_chkSkipMigrations.Checked) { args.Add("-SkipMigrations"); }
            if (_chkSkipBuild.Checked) { args.Add("-SkipBuild"); }
            if (_chkSkipPublicChecks.Checked) { args.Add("-SkipPublicChecks"); }
        }
        else
        {
            args.Add("-NoUi");
        }

        return args;
    }

    private void ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(_txtServerHost.Text))
        {
            throw new InvalidOperationException("Servidor es requerido.");
        }
        if (string.IsNullOrWhiteSpace(_txtServerUser.Text))
        {
            throw new InvalidOperationException("Usuario es requerido.");
        }
        if (string.IsNullOrWhiteSpace(_txtRemoteRepoPath.Text))
        {
            throw new InvalidOperationException("Repo remoto es requerido.");
        }
        if (string.IsNullOrWhiteSpace(_txtComposeFile.Text))
        {
            throw new InvalidOperationException("Compose file es requerido.");
        }
        if (GetSelectedAction() == "Deploy completo" && string.IsNullOrWhiteSpace(_txtBranch.Text))
        {
            throw new InvalidOperationException("Branch es requerido para deploy completo.");
        }
        if (GetSelectedMigrationMode() == "U" && string.IsNullOrWhiteSpace(_txtTenant.Text))
        {
            throw new InvalidOperationException("Tenant es requerido para modo U.");
        }

        if (GetEffectiveSshAuthMode() == "Password"
            && string.IsNullOrWhiteSpace(_txtSshPassword.Text)
            && !_chkInteractiveWindowForPassword.Checked)
        {
            throw new InvalidOperationException(
                "En modo Password debes ingresar una clave SSH o habilitar la terminal interactiva.");
        }
    }

    private async Task RunCapturedProcessAsync(List<string> args)
    {
        var psi = new ProcessStartInfo("pwsh")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        // El script deploy-hostinger.ps1 usa $env:HOLOS_SSH_PASSWORD para generar
        // el askpass script que OpenSSH ejecuta — pasarlo solo como arg no es suficiente.
        var sshPass = _txtSshPassword.Text;
        if (!string.IsNullOrEmpty(sshPass))
        {
            psi.Environment["HOLOS_SSH_PASSWORD"] = sshPass;
        }

        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }

        // Log del comando completo (sin contraseña)
        var safeArgs = args.Select(a =>
            (a == sshPass && !string.IsNullOrEmpty(sshPass)) ? "***" : a);
        AppendLog($"CMD: pwsh {string.Join(" ", safeArgs.Select(a => a.Contains(' ') ? $"\"{a}\"" : a))}");

        var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        _runningProcess = process;

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null) { AppendLog(e.Data); }
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                var clean = AnsiRegex().Replace(e.Data, string.Empty);
                if (!string.IsNullOrWhiteSpace(clean))
                    AppendLog("ERR: " + clean);
            }
        };

        _btnRun.Enabled = false;
        _btnStop.Enabled = true;

        AppendLog($"════════ INICIO [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ════════");
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        AppendLog($"════════ FIN código={process.ExitCode} [{DateTime.Now:HH:mm:ss}] ════════");
        _runningProcess = null;
        _btnRun.Enabled = true;
        _btnStop.Enabled = false;
    }

    private void StartInteractiveWindow(List<string> args)
    {
        var argLine = string.Join(' ', args.Select(QuoteForCommandLine));
        var cmd = $"& pwsh {argLine}; Write-Host ''; Write-Host 'Proceso finalizado. Presiona Enter para cerrar...'; Read-Host";

        var encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(cmd));
        var psi = new ProcessStartInfo("pwsh")
        {
            UseShellExecute = true,
            Arguments = $"-NoExit -EncodedCommand {encoded}"
        };

        Process.Start(psi);
    }

    private void StopCurrentProcess()
    {
        if (_runningProcess == null)
        {
            return;
        }

        try
        {
            _runningProcess.Kill(true);
            AppendLog("Proceso detenido por usuario.");
        }
        catch (Exception ex)
        {
            AppendLog("No se pudo detener el proceso: " + ex.Message);
        }
        finally
        {
            _runningProcess = null;
            _btnRun.Enabled = true;
            _btnStop.Enabled = false;
        }
    }

    private void OpenScriptsFolder()
    {
        try
        {
            var path = Path.Combine(_txtRepoPath.Text.Trim(), "scripts");
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("No se encontró la carpeta scripts: " + path);
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string GetSelectedAction() => _cmbAction.SelectedItem?.ToString() ?? "Deploy completo";

    private string GetSelectedSshAuth() => ParseOptionCode(_cmbSshAuth.SelectedItem?.ToString(), "Auto");

    private string GetEffectiveSshAuthMode()
    {
        var selected = GetSelectedSshAuth();
        if (selected.Equals("Auto", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(_txtSshPassword.Text))
        {
            return "Password";
        }

        return selected;
    }

    private string GetSelectedDeployTarget() => ParseOptionCode(_cmbDeployTarget.SelectedItem?.ToString(), "Both");

    private string GetSelectedMigrationMode() => ParseOptionCode(_cmbMigrationMode.SelectedItem?.ToString(), "B");

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

    private string LogFilePath =>
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "migrator_logs.txt"));

    private void AppendLog(string message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => AppendLog(message));
            return;
        }

        string logLine = $"[{DateTime.Now:HH:mm:ss}] {message}";
        _txtLog.AppendText(logLine + Environment.NewLine);

        try
        {
            var logDir = Path.GetDirectoryName(LogFilePath)!;
            Directory.CreateDirectory(logDir);
            File.AppendAllText(LogFilePath, logLine + Environment.NewLine, Encoding.UTF8);
        }
        catch { /* no bloquear UI si hay problema de permisos */ }
    }

    private void OpenLogFile()
    {
        try
        {
            var path = LogFilePath;
            if (!File.Exists(path))
            {
                MessageBox.Show("Todavía no hay archivo de log generado.\nEjecuta al menos una acción primero.", "Log no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string QuoteForCommandLine(string value)
    {
        return "'" + value.Replace("'", "''") + "'";
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"\x1b\[[0-9;]*[mGKHF]")]
    private static partial Regex AnsiRegex();

    private void LoadSettings()
    {
        try
        {
            if (!File.Exists(SettingsFilePath))
            {
                return;
            }

            var json = File.ReadAllText(SettingsFilePath);
            var settings = JsonSerializer.Deserialize<UiSettings>(json);
            if (settings == null)
            {
                return;
            }

            _chkRememberSshPassword.Checked = settings.RememberSshPassword;

            if (settings.RememberSshPassword && !string.IsNullOrWhiteSpace(settings.EncryptedSshPassword))
            {
                _txtSshPassword.Text = Unprotect(settings.EncryptedSshPassword);
            }
        }
        catch
        {
            // Ignorar archivos corruptos o inválidos y continuar con valores por defecto.
        }
    }

    private void SaveSettings()
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsFilePath)!;
            Directory.CreateDirectory(dir);

            var settings = new UiSettings
            {
                RememberSshPassword = _chkRememberSshPassword.Checked,
                EncryptedSshPassword = _chkRememberSshPassword.Checked && !string.IsNullOrWhiteSpace(_txtSshPassword.Text)
                    ? Protect(_txtSshPassword.Text)
                    : null
            };

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }
        catch
        {
            // Si falla persistencia local, no bloquear la ejecución principal.
        }
    }

    private static string Protect(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        var protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(protectedBytes);
    }

    private static string Unprotect(string encryptedBase64)
    {
        var protectedBytes = Convert.FromBase64String(encryptedBase64);
        var bytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(bytes);
    }

    private bool _advancedVisible = false;

    private void ConfigureCyberpunkUI()
    {
        // Paleta Cyberpunk
        var bgDark = Color.FromArgb(9, 9, 11);
        var panelDark = Color.FromArgb(18, 18, 23);
        var neonGreen = Color.FromArgb(0, 255, 65);
        var neonCyan = Color.FromArgb(0, 255, 255);
        var terminalFont = new Font("Consolas", 10F, FontStyle.Regular);
        var titleFont = new Font("Consolas", 11F, FontStyle.Bold);

        this.BackColor = bgDark;

        foreach (var grp in new[] { _grpGeneral, _grpSsh, _grpOps })
        {
            if (grp == null) continue;
            grp.BackColor = panelDark;
            grp.FlatStyle = FlatStyle.Flat;
            grp.Font = titleFont;
            grp.ForeColor = neonCyan;
            grp.Padding = new Padding(15);
        }

        foreach (var tbl in new[] { _tblGeneral, _tblSsh, _tblOps })
        {
            if (tbl == null) continue;
            tbl.Font = terminalFont;
            tbl.BackColor = panelDark;
            tbl.ForeColor = neonGreen;
        }

        if (_panelHeader != null)
        {
            _panelHeader.BackColor = Color.FromArgb(5, 5, 5);
        }
        if (_lblAppTitle != null)
        {
            _lblAppTitle.ForeColor = neonGreen;
            _lblAppTitle.Font = new Font("Consolas", 16F, FontStyle.Bold);
            _lblAppTitle.Text = "> HOLOS_MIGRATOR_SYS_CTL";
        }

        foreach (Control c in this.Controls) { ApplyCyberpunkKids(c, bgDark, panelDark, neonGreen, neonCyan); }

        var btnAdvanced = new Button
        {
            Text = "[ SYS.ADVANCED ]",
            BackColor = panelDark,
            ForeColor = neonCyan,
            FlatStyle = FlatStyle.Flat,
            Font = titleFont,
            AutoSize = true,
            MinimumSize = new Size(160, 44),
            Margin = new Padding(0, 10, 10, 10),
            Cursor = Cursors.Hand
        };
        btnAdvanced.FlatAppearance.BorderColor = neonCyan;
        btnAdvanced.FlatAppearance.BorderSize = 1;

        btnAdvanced.Click += (_, _) =>
        {
            _advancedVisible = !_advancedVisible;
            btnAdvanced.BackColor = _advancedVisible ? neonCyan : panelDark;
            btnAdvanced.ForeColor = _advancedVisible ? panelDark : neonCyan;
            UpdateAdvancedVisibility();
        };

        if (_flpHeaderActions != null)
        {
            _flpHeaderActions.Controls.Add(btnAdvanced);
            _flpHeaderActions.Controls.SetChildIndex(btnAdvanced, 0);
        }

        UpdateAdvancedVisibility();
    }

    private void ApplyCyberpunkKids(Control parent, Color bgDark, Color panelDark, Color neonGreen, Color neonCyan)
    {
        if (parent == null) return;
        foreach (Control c in parent.Controls)
        {
            c.ForeColor = neonGreen;

            if (c is Panel pnl && pnl.Name == "_panelSshPassword")
            {
                pnl.BackColor = bgDark;
                pnl.BorderStyle = BorderStyle.FixedSingle;
            }

            if (c is TextBox txt && txt.Name != "_txtLog")
            {
                txt.BorderStyle = txt.Name == "_txtSshPassword" ? BorderStyle.None : BorderStyle.FixedSingle;
                txt.BackColor = bgDark;
                txt.ForeColor = neonCyan;
                if (txt.Name == "_txtSshPassword")
                {
                    txt.Location = new Point(5, txt.Location.Y);
                    if (txt.Parent != null)
                    {
                        txt.Width = txt.Parent.Width - 40;
                    }
                }
            }
            if (c is ComboBox cmb)
            {
                cmb.FlatStyle = FlatStyle.Flat;
                cmb.BackColor = bgDark;
                cmb.ForeColor = neonCyan;
            }
            if (c is NumericUpDown num)
            {
                num.BackColor = bgDark;
                num.ForeColor = neonCyan;
                num.BorderStyle = BorderStyle.FixedSingle;
            }
            if (c is Button btn && (btn.Name == "_btnRun" || btn.Name == "_btnStop" || btn.Name == "_btnOpenScripts"))
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.Font = new Font("Consolas", 10F, FontStyle.Bold);
                btn.AutoSize = true;
                btn.MinimumSize = new Size(130, 44);

                if (btn.Name == "_btnRun")
                {
                    btn.BackColor = Color.FromArgb(0, 40, 0);
                    btn.ForeColor = neonGreen;
                    btn.FlatAppearance.BorderColor = neonGreen;
                    btn.Text = "▶  EJECUTAR ";
                }
                else if (btn.Name == "_btnStop")
                {
                    btn.BackColor = Color.FromArgb(60, 10, 10);
                    btn.ForeColor = Color.FromArgb(255, 120, 120); // Rojo brillante
                    btn.FlatAppearance.BorderColor = Color.FromArgb(255, 60, 60);
                    btn.Text = "⏹  DETENER ";
                }
                else if (btn.Name == "_btnOpenScripts")
                {
                    btn.BackColor = panelDark;
                    btn.ForeColor = neonCyan;
                    btn.FlatAppearance.BorderColor = neonCyan;
                    btn.Text = "📂  SCRIPTS ";
                }
            }
            if (c is Button showBtn && showBtn.Name == "_btnShowPassword")
            {
                showBtn.FlatStyle = FlatStyle.Flat;
                showBtn.FlatAppearance.BorderSize = 0;
                showBtn.BackColor = bgDark;
                showBtn.ForeColor = neonCyan;
            }
            ApplyCyberpunkKids(c, bgDark, panelDark, neonGreen, neonCyan);
        }
    }

    private void UpdateAdvancedVisibility()
    {
        // General (Avanzados)
        SetRowVisible(_tblGeneral, _txtRemoteRepoPath, _advancedVisible);
        SetRowVisible(_tblGeneral, _txtBranch, _advancedVisible);
        SetRowVisible(_tblGeneral, _txtComposeFile, _advancedVisible);
        SetRowVisible(_tblGeneral, _txtGitToken, _advancedVisible);

        // SSH (Avanzados)
        SetRowVisible(_tblSsh, _numSshPort, _advancedVisible);
        SetRowVisible(_tblSsh, _txtSshKeyPath, _advancedVisible);

        if (_chkSshBatchMode != null) _chkSshBatchMode.Visible = _advancedVisible;
        if (_chkInteractiveWindowForPassword != null) _chkInteractiveWindowForPassword.Visible = _advancedVisible;
    }

    private void SetRowVisible(TableLayoutPanel panel, Control ctrl, bool visible)
    {
        if (panel == null || ctrl == null) return;
        var pos = panel.GetPositionFromControl(ctrl);
        if (pos.Row >= 0)
        {
            ctrl.Visible = visible;
            // Ocultar también la etiqueta (Label) que suele estar en la misma fila (columna 0)
            for (int c = 0; c < panel.ColumnCount; c++)
            {
                var sibling = panel.GetControlFromPosition(c, pos.Row);
                if (sibling != null) sibling.Visible = visible;
            }
        }
    }

    private sealed class UiSettings
    {
        public bool RememberSshPassword { get; set; }

        public string? EncryptedSshPassword { get; set; }
    }
}
