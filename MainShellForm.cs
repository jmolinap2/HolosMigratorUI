using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using HolosMigratorUI.Core;

namespace HolosMigratorUI;

public partial class MainShellForm : Form
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private Process? _runningProcess;
    private DateTime _currentRunStartedAt;
    private string SettingsFilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "HolosMigratorUI",
        "settings.json");

    public MainShellForm()
    {
        InitializeComponent();
        BindEvents();
        LoadSettings();
        LoadEnvVariables();
        ApplyUiState();
        UpdateAdvancedButtonState();
        UpdateAdvancedVisibility();
        UpdateEnvironmentVisuals();
        FormClosing += (_, _) => SaveSettings();
        AppendLog("✅ Sistema listo. Ingresa tu Password SSH y presiona '▶ EJECUTAR' arriba a la derecha.");
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
        _btnAdvanced.Click += (_, _) => ToggleAdvancedMode();
        _btnRun.Click += async (_, _) => await RunSelectedActionAsync();
        _btnStop.Click += (_, _) => StopCurrentProcess();
        _btnOpenScripts.Click += (_, _) => OpenScriptsFolder();
        _btnOpenLog.Click += (_, _) => OpenLogFile();
        _btnControlCenter.Click += (_, _) => OpenControlCenter();
        _btnEnvironment.Click += (_, _) => CycleEnvironment();
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

        _currentRunStartedAt = DateTime.Now;

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

            _panelStatus.Visible = false;
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

        if (_state.CurrentEnvironment == DeploymentEnvironment.Production)
        {
            if (_chkSkipPublicChecks.Checked)
            {
                throw new InvalidOperationException("En Production no se permite omitir checks públicos.");
            }

            if (GetSelectedAction() == "Deploy completo" && _chkSkipMigrations.Checked)
            {
                throw new InvalidOperationException("En Production no se permite omitir migraciones.");
            }
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
            (a == sshPass && !string.IsNullOrEmpty(sshPass)) ? "***" : a).ToList();
        var cmdText = $"CMD: pwsh {string.Join(" ", safeArgs.Select(a => a.Contains(' ') ? $"\"{a}\"" : a))}";
        AppendLog(LogSecurity.Sanitize(cmdText));

        var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        _runningProcess = process;

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                AppendLog(e.Data, "STDOUT");
                this.Invoke(() => UpdateProgressFromLog(e.Data));
            }
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                var clean = AnsiRegex().Replace(e.Data, string.Empty);
                if (!string.IsNullOrWhiteSpace(clean))
                {
                    AppendLog("ERR: " + clean, "STDERR");
                    this.Invoke(() => UpdateProgressFromLog(clean));
                }
            }
        };

        _btnRun.Enabled = false;
        _btnStop.Enabled = true;
        _progressBar.Style = ProgressBarStyle.Continuous;
        _progressBar.Minimum = 0;
        _progressBar.Maximum = 100;
        _progressBar.Value = 0;
        _progressBar.Visible = true;
        _panelStatus.Visible = false;

        AppendLog($"════════ INICIO [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ════════");
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        _progressBar.Visible = false;
        AppendLog($"════════ FIN código={process.ExitCode} [{DateTime.Now:HH:mm:ss}] ════════");

        bool success = process.ExitCode == 0;
        _panelStatus.BackColor = success ? Color.FromArgb(0, 40, 10) : Color.FromArgb(60, 10, 10);
        _lblStatus.ForeColor = success ? Color.FromArgb(0, 255, 65) : Color.FromArgb(255, 80, 80);
        _lblStatus.Text = success
            ? "✅  DEPLOY COMPLETADO CORRECTAMENTE"
            : $"❌  ERROR — Proceso terminó con código {process.ExitCode}. Revisa la salida arriba.";
        _panelStatus.Visible = true;

        _state.AddRun(new OperationRunSummary(
            StartedAt: _currentRunStartedAt,
            EndedAt: DateTime.Now,
            ExitCode: process.ExitCode,
            Action: GetSelectedAction(),
            DeployTarget: GetSelectedDeployTarget(),
            MigrationMode: GetSelectedMigrationMode(),
            Environment: _state.CurrentEnvironment));

        _runningProcess = null;
        _btnRun.Enabled = true;
        _btnStop.Enabled = false;
    }

    private static readonly (string Pattern, int Value)[] _progressSteps =
    [
        ("[INFO] Verificando docker compose",     5),
        ("[INFO] Actualizando codigo",            10),
        ("[INFO] SkipPull activo",               15),
        ("[INFO] Levantando target",             25),
        ("[INFO] Asegurando SQL Server",         70),
        ("[INFO] Ejecutando migraciones",        75),
        ("[INFO] SkipMigrations activo",         80),
        ("[INFO] Estado final de servicios",     88),
        ("[INFO] Smoke checks internos",         90),
        ("[OK] Deploy remoto completado",        95),
        ("[INFO] Smoke checks publicos",         97),
        ("[OK] Automatizacion finalizada",      100),
    ];

    private void UpdateProgressFromLog(string line)
    {
        // Docker build layers (#N) contribuyen al rango 25-68%
        if (line.StartsWith('#') && _progressBar.Value is >= 25 and < 68)
        {
            // Avanza 1% por cada línea de build layer, tope en 68
            var next = Math.Min(_progressBar.Value + 1, 68);
            if (next > _progressBar.Value) _progressBar.Value = next;
            return;
        }

        foreach (var (pattern, value) in _progressSteps)
        {
            if (line.Contains(pattern) && value > _progressBar.Value)
            {
                _progressBar.Value = value;
                return;
            }
        }
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

    private void AppendLog(string message, string source = "UI", bool evaluatePolicy = true)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => AppendLog(message, source, evaluatePolicy));
            return;
        }

        var now = DateTime.Now;
        var sanitizedMessage = LogSecurity.Sanitize(message);
        string logLine = $"[{now:HH:mm:ss}] {sanitizedMessage}";
        _txtLog.AppendText(logLine + Environment.NewLine);

        var severity = LogClassifier.Classify(sanitizedMessage);
        _state.AddLog(new LogEntry(now, source, message, sanitizedMessage, severity));

        if (evaluatePolicy)
        {
            EvaluateRuntimePolicy(sanitizedMessage);
        }

        try
        {
            var logDir = Path.GetDirectoryName(LogFilePath)!;
            Directory.CreateDirectory(logDir);
            File.AppendAllText(LogFilePath, logLine + Environment.NewLine, Encoding.UTF8);
        }
        catch { /* no bloquear UI si hay problema de permisos */ }
    }

    private void EvaluateRuntimePolicy(string line)
    {
        var results = EnvironmentPolicy.ValidateRuntimeLine(_state.CurrentEnvironment, line);
        foreach (var r in results)
        {
            _state.AddAlert(new AlertEvent(
                CreatedAt: DateTime.Now,
                Rule: "EnvironmentPolicy",
                Severity: r.IsBlocking ? "Critical" : "Warning",
                Message: r.Message,
                IsActive: true));

            AppendLog($"POLICY {(r.IsBlocking ? "BLOCK" : "WARN")}: {r.Message}", "POLICY", false);
        }
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

            if (!string.IsNullOrWhiteSpace(settings.Environment)
                && Enum.TryParse<DeploymentEnvironment>(settings.Environment, out var env))
            {
                _state.CurrentEnvironment = env;
            }

            UpdateEnvironmentVisuals();
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
                    : null,
                Environment = _state.CurrentEnvironment.ToString()
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

    private void ToggleAdvancedMode()
    {
        _advancedVisible = !_advancedVisible;
        UpdateAdvancedButtonState();
        UpdateAdvancedVisibility();
    }

    private void UpdateAdvancedButtonState()
    {
        _btnAdvanced.Text = _advancedVisible ? "[ SYS.ADVANCED: ON ]" : "[ SYS.ADVANCED ]";
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

        public string? Environment { get; set; }
    }

    private void OpenControlCenter()
    {
        using var center = new ControlCenterForm(
            hostProvider: () => _txtServerHost.Text.Trim(),
            sshPortProvider: () => (int)_numSshPort.Value);

        center.ShowDialog(this);
        UpdateEnvironmentVisuals();
    }

    private void CycleEnvironment()
    {
        var next = _state.CurrentEnvironment switch
        {
            DeploymentEnvironment.Development => DeploymentEnvironment.Staging,
            DeploymentEnvironment.Staging => DeploymentEnvironment.Production,
            _ => DeploymentEnvironment.Development
        };

        _state.CurrentEnvironment = next;
        UpdateEnvironmentVisuals();
        SaveSettings();
        AppendLog($"Entorno activo: {_state.CurrentEnvironment}", "ENV", false);
    }

    private void UpdateEnvironmentVisuals()
    {
        var profile = EnvironmentPolicy.GetProfile(_state.CurrentEnvironment);
        _btnEnvironment.Text = $"ENV: {profile.RiskLabel}";
        _lblEnvironmentRisk.Text = $"RISK: {profile.RiskLabel}";

        var color = ColorTranslator.FromHtml(profile.RiskColor);
        _btnEnvironment.ForeColor = color;
        _btnEnvironment.FlatAppearance.BorderColor = color;
        _lblEnvironmentRisk.ForeColor = color;
    }
}
