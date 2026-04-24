using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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
        LoadEnvVariables();
        BindEvents();
        LoadSettings();
        ApplyUiState();
        FormClosing += (_, _) => SaveSettings();
        AppendLog("UI inicializada. Configura opciones y presiona Ejecutar.");
    }

    private void LoadEnvVariables()
    {
        try
        {
            DotNetEnv.Env.Load();
            
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
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error cargando .env: {ex.Message}");
        }
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
        _txtTenant.Enabled = migrationMode == "U" && !skippingMigrations;
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

        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }

        var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        _runningProcess = process;

        process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data)) { AppendLog(e.Data); }
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data)) { AppendLog("ERR: " + e.Data); }
        };

        _btnRun.Enabled = false;
        _btnStop.Enabled = true;

        AppendLog("Ejecutando proceso...");
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        AppendLog($"Proceso finalizado con código: {process.ExitCode}");
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

    private void AppendLog(string message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => AppendLog(message));
            return;
        }

        _txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
    }

    private static string QuoteForCommandLine(string value)
    {
        return "'" + value.Replace("'", "''") + "'";
    }

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

    private sealed class UiSettings
    {
        public bool RememberSshPassword { get; set; }

        public string? EncryptedSshPassword { get; set; }
    }
}
