using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using HolosMigratorUI.Core;

namespace HolosMigratorWpf;

public partial class MainWindow : Window
{
    private readonly AppStateStore _state = AppStateStore.Instance;
    private readonly ObservableCollection<OperationRunRow> _recentRuns = [];
    private readonly Dictionary<string, TextBlock> _serviceLabels;
    private readonly DispatcherTimer _statusTimer = new() { Interval = TimeSpan.FromSeconds(30) };
    private Process? _runningProcess;
    private DateTime _currentRunStartedAt;
    private bool _advancedVisible;
    private bool _isRefreshingStatus;

    private string RepositoryRoot => FindRepositoryRoot() ?? Directory.GetCurrentDirectory();

    private string SettingsFilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "HolosMigratorWpf",
        "settings.json");

    private string LogFilePath => Path.Combine(RepositoryRoot, "migrator_logs.txt");

    public MainWindow()
    {
        InitializeComponent();

        _serviceLabels = new()
        {
            ["sql"] = SqlStatusText,
            ["api"] = ApiStatusText,
            ["front"] = FrontStatusText
        };

        RecentRunsGrid.ItemsSource = _recentRuns;

        LoadSettings();
        LoadEnvVariables();
        ApplyAdvancedVisibility();
        ApplyUiState();
        ApplyEnvironmentVisuals();
        ShowPage(PageOperations, NavOperations);

        _statusTimer.Tick += async (_, _) => await RefreshEnvironmentStatusAsync();
        _statusTimer.Start();
        Closing += (_, _) =>
        {
            _statusTimer.Stop();
            SaveSettings();
        };

        AppendLog("Sistema listo. Configura SSH y ejecuta Apply / Ejecutar.");
        _ = RefreshEnvironmentStatusAsync();
    }

    private void Nav_Click(object sender, RoutedEventArgs e)
    {
        if (sender == NavOperations) ShowPage(PageOperations, NavOperations);
        if (sender == NavDashboard) ShowPage(PageDashboard, NavDashboard);
        if (sender == NavStorage) ShowPage(PageStorage, NavStorage);
        if (sender == NavLogs) ShowPage(PageLogs, NavLogs);
        if (sender == NavSettings) ShowPage(PageSettings, NavSettings);
    }

    private void ShowPage(Grid page, Button nav)
    {
        foreach (var grid in new[] { PageOperations, PageDashboard, PageStorage, PageLogs, PageSettings })
        {
            grid.Visibility = grid == page ? Visibility.Visible : Visibility.Collapsed;
        }

        foreach (var button in new[] { NavOperations, NavDashboard, NavStorage, NavLogs, NavSettings })
        {
            button.Background = button == nav ? Brush("#2A2A2A") : Brushes.Transparent;
            button.Foreground = button == nav ? Brush("#FFFFFF") : Brush("#B8B8B8");
        }

        if (page == PageDashboard)
        {
            _ = RefreshDashboardAsync();
        }

        if (page == PageStorage)
        {
            _ = RefreshStorageAsync();
        }
    }

    private void LoadEnvVariables()
    {
        try
        {
            var envPath = FindEnvFile();
            if (envPath == null)
            {
                AppendLog(".env no encontrado. Se mantienen valores por defecto.");
                UpdateSettingsSummary();
                return;
            }

            foreach (var (key, value) in ParseEnvFile(envPath))
            {
                switch (key)
                {
                    case "SERVER_HOST":
                        ServerHostText.Text = value;
                        break;
                    case "SERVER_USER":
                        ServerUserText.Text = value;
                        break;
                    case "SSH_PORT":
                        SshPortText.Text = value;
                        break;
                    case "SSH_PASSWORD":
                        if (string.IsNullOrWhiteSpace(SshPasswordBox.Password))
                        {
                            SshPasswordBox.Password = value;
                        }
                        break;
                    case "REPO_LOCAL":
                        RepoLocalText.Text = value;
                        break;
                    case "SSH_KEY_PATH":
                        SshKeyPathText.Text = value;
                        break;
                    case "GIT_TOKEN":
                        GitTokenBox.Password = value;
                        break;
                }
            }

            AppendLog($".env cargado desde: {envPath}");
            UpdateSettingsSummary();
        }
        catch (Exception ex)
        {
            AppendLog("Error cargando .env: " + ex.Message);
        }
    }

    private static IEnumerable<(string Key, string Value)> ParseEnvFile(string path)
    {
        foreach (var rawLine in File.ReadAllLines(path))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var idx = line.IndexOf('=');
            if (idx <= 0)
            {
                continue;
            }

            var key = line[..idx].Trim();
            var value = line[(idx + 1)..].Trim().Trim('"');
            yield return (key, value);
        }
    }

    private static string? FindEnvFile()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, ".env");
            if (File.Exists(candidate))
            {
                return candidate;
            }

            candidate = Path.Combine(dir.FullName, "..", ".env");
            var full = Path.GetFullPath(candidate);
            if (File.Exists(full))
            {
                return full;
            }

            dir = dir.Parent;
        }

        return null;
    }

    private static string? FindRepositoryRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "HolosMigratorUI.csproj")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        var parent = Directory.GetParent(AppContext.BaseDirectory);
        while (parent != null)
        {
            var candidate = Path.Combine(parent.FullName, "HolosMigratorUI.csproj");
            if (File.Exists(candidate))
            {
                return parent.FullName;
            }

            parent = parent.Parent;
        }

        return null;
    }

    private void FormState_Changed(object sender, EventArgs e) => ApplyUiState();

    private async void ServerField_LostFocus(object sender, RoutedEventArgs e)
    {
        await RefreshEnvironmentStatusAsync();
    }

    private void SshPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ApplyUiState();
    }

    private void ApplyUiState()
    {
        var action = GetSelectedAction();
        var migrationMode = GetSelectedMigrationMode();
        var skippingMigrations = action == "Deploy completo" && SkipMigrationsCheck.IsChecked == true;
        var sshAuth = GetSelectedSshAuth();
        var effectiveAuth = GetEffectiveSshAuthMode();

        DeployTargetCombo.IsEnabled = action == "Deploy completo";
        MigrationModeCombo.IsEnabled = !skippingMigrations;
        TenantText.IsEnabled = migrationMode == "U" && !skippingMigrations;

        SkipPullCheck.IsEnabled = action == "Deploy completo";
        SkipMigrationsCheck.IsEnabled = action == "Deploy completo";
        SkipBuildCheck.IsEnabled = action == "Deploy completo";
        SkipPublicChecksCheck.IsEnabled = action == "Deploy completo";

        SshKeyPathText.IsEnabled = sshAuth != "Password";
        SshBatchModeCheck.IsEnabled = sshAuth != "Password";
        InteractivePasswordCheck.IsEnabled = effectiveAuth == "Password" && string.IsNullOrWhiteSpace(SshPasswordBox.Password);

        if (effectiveAuth != "Password")
        {
            InteractivePasswordCheck.IsChecked = false;
        }
    }

    private async void RunButton_Click(object sender, RoutedEventArgs e)
    {
        await RunSelectedActionAsync();
    }

    private async Task RunSelectedActionAsync()
    {
        if (_runningProcess != null)
        {
            AppendLog("Ya hay un proceso en ejecucion.");
            return;
        }

        _currentRunStartedAt = DateTime.Now;

        try
        {
            var scriptPath = GetScriptPath();
            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException("No se encontro el script: " + scriptPath);
            }

            ValidateInputs();
            var args = BuildPowerShellArgs(scriptPath);

            SaveSettings();
            AppendLog($"Accion: {GetSelectedAction()}");
            AppendLog($"Script: {scriptPath}");
            AppendLog($"Auth SSH efectivo: {GetEffectiveSshAuthMode()}");

            if (GetEffectiveSshAuthMode() == "Password"
                && string.IsNullOrWhiteSpace(SshPasswordBox.Password)
                && InteractivePasswordCheck.IsChecked == true)
            {
                StartInteractiveWindow(args);
                AppendLog("Se abrio una terminal interactiva para ingresar la clave SSH.");
                return;
            }

            await RunCapturedProcessAsync(args);
        }
        catch (Exception ex)
        {
            AppendLog("ERROR: " + ex.Message);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private string GetScriptPath()
    {
        var repo = RepoLocalText.Text.Trim();
        if (string.IsNullOrWhiteSpace(repo))
        {
            throw new InvalidOperationException("Debes indicar Repo local.");
        }

        var scriptFile = GetSelectedAction() == "Deploy completo"
            ? "deploy-hostinger.ps1"
            : "migrate-hostinger-ui.ps1";

        return Path.Combine(repo, "scripts", scriptFile);
    }

    private List<string> BuildPowerShellArgs(string scriptPath)
    {
        var args = new List<string>
        {
            "-NoProfile",
            "-ExecutionPolicy", "Bypass",
            "-File", scriptPath,
            "-ServerHost", ServerHostText.Text.Trim(),
            "-ServerUser", ServerUserText.Text.Trim(),
            "-SshPort", SshPortText.Text.Trim(),
            "-SshAuthMode", GetEffectiveSshAuthMode(),
            "-RemoteRepoPath", RemoteRepoPathText.Text.Trim(),
            "-ComposeFile", ComposeFileText.Text.Trim(),
            "-MigrationMode", GetSelectedMigrationMode()
        };

        if (!string.IsNullOrWhiteSpace(SshKeyPathText.Text))
        {
            args.Add("-SshKeyPath");
            args.Add(SshKeyPathText.Text.Trim());
        }

        if (SshBatchModeCheck.IsChecked == true)
        {
            args.Add("-SshBatchMode");
        }

        if (GetEffectiveSshAuthMode() == "Password" && !string.IsNullOrWhiteSpace(SshPasswordBox.Password))
        {
            args.Add("-SshPassword");
            args.Add(SshPasswordBox.Password);
        }

        if (GetSelectedMigrationMode() == "U")
        {
            args.Add("-TenantIdentifier");
            args.Add(TenantText.Text.Trim());
        }

        if (GetSelectedAction() == "Deploy completo")
        {
            args.Add("-Branch");
            args.Add(BranchText.Text.Trim());
            args.Add("-DeployTarget");
            args.Add(GetSelectedDeployTarget());

            if (!string.IsNullOrWhiteSpace(GitTokenBox.Password))
            {
                args.Add("-GitToken");
                args.Add(GitTokenBox.Password);
            }

            if (SkipPullCheck.IsChecked == true) args.Add("-SkipPull");
            if (SkipMigrationsCheck.IsChecked == true) args.Add("-SkipMigrations");
            if (SkipBuildCheck.IsChecked == true) args.Add("-SkipBuild");
            if (SkipPublicChecksCheck.IsChecked == true) args.Add("-SkipPublicChecks");
        }
        else
        {
            args.Add("-NoUi");
        }

        return args;
    }

    private void ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(ServerHostText.Text)) throw new InvalidOperationException("Servidor es requerido.");
        if (string.IsNullOrWhiteSpace(ServerUserText.Text)) throw new InvalidOperationException("Usuario es requerido.");
        if (string.IsNullOrWhiteSpace(RemoteRepoPathText.Text)) throw new InvalidOperationException("Repo remoto es requerido.");
        if (string.IsNullOrWhiteSpace(ComposeFileText.Text)) throw new InvalidOperationException("Compose file es requerido.");
        if (!int.TryParse(SshPortText.Text.Trim(), out _)) throw new InvalidOperationException("Puerto SSH invalido.");
        if (GetSelectedAction() == "Deploy completo" && string.IsNullOrWhiteSpace(BranchText.Text)) throw new InvalidOperationException("Branch es requerido.");
        if (GetSelectedMigrationMode() == "U" && string.IsNullOrWhiteSpace(TenantText.Text)) throw new InvalidOperationException("Tenant es requerido para modo U.");

        if (_state.CurrentEnvironment == DeploymentEnvironment.Production)
        {
            if (SkipPublicChecksCheck.IsChecked == true)
            {
                throw new InvalidOperationException("En Production no se permite omitir checks publicos.");
            }

            if (GetSelectedAction() == "Deploy completo" && SkipMigrationsCheck.IsChecked == true)
            {
                throw new InvalidOperationException("En Production no se permite omitir migraciones.");
            }
        }

        if (GetEffectiveSshAuthMode() == "Password"
            && string.IsNullOrWhiteSpace(SshPasswordBox.Password)
            && InteractivePasswordCheck.IsChecked != true)
        {
            throw new InvalidOperationException("En modo Password debes ingresar clave SSH o habilitar terminal interactiva.");
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

        if (!string.IsNullOrWhiteSpace(SshPasswordBox.Password))
        {
            psi.Environment["HOLOS_SSH_PASSWORD"] = SshPasswordBox.Password;
        }

        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }

        AppendLog("CMD: pwsh " + string.Join(" ", RedactArgs(args).Select(QuoteForDisplay)));

        var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        _runningProcess = process;
        RunButton.IsEnabled = false;
        StopButton.IsEnabled = true;
        RunProgressBar.Value = 0;
        StatusBadge.Text = "Ejecutando...";

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data == null) return;
            Dispatcher.Invoke(() =>
            {
                AppendLog(e.Data, "STDOUT");
                UpdateProgressFromLog(e.Data);
            });
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data == null) return;
            Dispatcher.Invoke(() =>
            {
                var clean = AnsiRegex().Replace(e.Data, string.Empty);
                if (!string.IsNullOrWhiteSpace(clean))
                {
                    AppendLog("ERR: " + clean, "STDERR");
                    UpdateProgressFromLog(clean);
                }
            });
        };

        AppendLog($"======== INICIO [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ========");
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();

        var exitCode = process.ExitCode;
        AppendLog($"======== FIN codigo={exitCode} [{DateTime.Now:HH:mm:ss}] ========");
        FinishRun(exitCode);
    }

    private void FinishRun(int exitCode)
    {
        var success = exitCode == 0;
        RunProgressBar.Value = success ? 100 : RunProgressBar.Value;
        StatusBadge.Text = success
            ? "Deploy completado correctamente."
            : $"Proceso termino con codigo {exitCode}. Revisa el log.";
        StatusBadge.Foreground = success ? Brush("#4ED68A") : Brush("#FF8181");

        _state.AddRun(new OperationRunSummary(
            _currentRunStartedAt,
            DateTime.Now,
            exitCode,
            GetSelectedAction(),
            GetSelectedDeployTarget(),
            GetSelectedMigrationMode(),
            _state.CurrentEnvironment));

        RefreshRecentRuns();
        _runningProcess = null;
        RunButton.IsEnabled = true;
        StopButton.IsEnabled = true;
    }

    private static readonly (string Pattern, int Value)[] ProgressSteps =
    [
        ("[INFO] Verificando docker compose", 5),
        ("[INFO] Actualizando codigo", 10),
        ("[INFO] SkipPull activo", 15),
        ("[INFO] Levantando target", 25),
        ("[INFO] Asegurando SQL Server", 70),
        ("[INFO] Ejecutando migraciones", 75),
        ("[INFO] SkipMigrations activo", 80),
        ("[INFO] Estado final de servicios", 88),
        ("[INFO] Smoke checks internos", 90),
        ("[OK] Deploy remoto completado", 95),
        ("[INFO] Smoke checks publicos", 97),
        ("[OK] Automatizacion finalizada", 100),
    ];

    private void UpdateProgressFromLog(string line)
    {
        if (line.StartsWith('#') && RunProgressBar.Value is >= 25 and < 68)
        {
            RunProgressBar.Value = Math.Min(RunProgressBar.Value + 1, 68);
            return;
        }

        foreach (var (pattern, value) in ProgressSteps)
        {
            if (line.Contains(pattern, StringComparison.OrdinalIgnoreCase) && value > RunProgressBar.Value)
            {
                RunProgressBar.Value = value;
                return;
            }
        }
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        if (_runningProcess == null)
        {
            AppendLog("No hay proceso en ejecucion para detener.");
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
            RunButton.IsEnabled = true;
            StopButton.IsEnabled = true;
        }
    }

    private void StartInteractiveWindow(List<string> args)
    {
        var argLine = string.Join(' ', args.Select(QuoteForCommandLine));
        var cmd = $"& pwsh {argLine}; Write-Host ''; Write-Host 'Proceso finalizado. Presiona Enter para cerrar...'; Read-Host";
        var encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(cmd));

        Process.Start(new ProcessStartInfo("pwsh")
        {
            UseShellExecute = true,
            Arguments = $"-NoExit -EncodedCommand {encoded}"
        });
    }

    private async Task RefreshEnvironmentStatusAsync()
    {
        if (_isRefreshingStatus) return;
        _isRefreshingStatus = true;

        try
        {
            var host = ServerHostText.Text.Trim();
            var port = GetSshPort();
            var profile = EnvironmentPolicy.GetProfile(_state.CurrentEnvironment);

            if (string.IsNullOrWhiteSpace(host))
            {
                EnvironmentStatusText.Text = $"VPS: HOST SIN CONFIGURAR | {profile.RiskLabel}";
                EnvironmentStatusText.Foreground = Brush("#FFD166");
                return;
            }

            var reachable = await HealthCheckService.IsHostReachableAsync(host, port);
            EnvironmentStatusText.Text = reachable
                ? $"APP: {GetRuntimeModeLabel()} | VPS: EN LINEA | OBJETIVO: {profile.RiskLabel} | {DateTime.Now:HH:mm:ss}"
                : $"APP: {GetRuntimeModeLabel()} | VPS: SIN CONEXION | OBJETIVO: {profile.RiskLabel} | {DateTime.Now:HH:mm:ss}";
            EnvironmentStatusText.Foreground = reachable ? Brush("#4ED68A") : Brush("#FF8181");
        }
        catch
        {
            EnvironmentStatusText.Text = "VPS: ESTADO DESCONOCIDO";
            EnvironmentStatusText.Foreground = Brush("#FFD166");
        }
        finally
        {
            _isRefreshingStatus = false;
        }
    }

    private async void RefreshDashboardButton_Click(object sender, RoutedEventArgs e)
    {
        await RefreshDashboardAsync();
    }

    private async Task RefreshDashboardAsync()
    {
        var host = ServerHostText.Text.Trim();
        var port = GetSshPort();
        var user = ServerUserText.Text.Trim();
        var keyPath = NullIfEmpty(SshKeyPathText.Text.Trim());
        var password = NullIfEmpty(SshPasswordBox.Password);
        var reachable = !string.IsNullOrWhiteSpace(host) && await HealthCheckService.IsHostReachableAsync(host, port);

        IReadOnlyDictionary<string, string>? states = null;
        IReadOnlyDictionary<string, string>? metrics = null;
        var canUseSsh = reachable && !string.IsNullOrWhiteSpace(user) && (keyPath != null || password != null);

        if (canUseSsh)
        {
            states = await HealthCheckService.TryGetDockerServiceStatesAsync(host, user, port, keyPath, password);
            metrics = await HealthCheckService.TryGetServerResourceMetricsAsync(host, user, port, keyPath, password);
        }

        if (states != null)
        {
            foreach (var entry in states)
            {
                _state.SetServiceState(entry.Key, entry.Value);
            }
        }

        SetMetric(CpuText, metrics, "cpu", reachable ? "Sin datos remotos" : "Sin conexion");
        SetMetric(MemoryText, metrics, "memory", reachable ? "Sin datos remotos" : "Sin conexion");
        SetMetric(DiskText, metrics, "disk", reachable ? "Sin datos remotos" : "Sin conexion");
        SetMetric(UptimeText, metrics, "uptime", reachable ? "Sin datos remotos" : "Sin conexion");

        foreach (var key in _serviceLabels.Keys)
        {
            var state = states != null && states.TryGetValue(key, out var remote) ? remote : _state.GetServiceState(key);
            SetServiceLabel(_serviceLabels[key], state);
        }

        var serviceStates = new[] { _state.GetServiceState("sql"), _state.GetServiceState("api"), _state.GetServiceState("front") };
        var active = serviceStates.Count(s => s == "running");
        OverallStatusText.Text = !reachable
            ? "Estado VPS: SIN CONEXION SSH"
            : active == 3
                ? "Estado VPS: OPERATIVO"
                : $"Estado VPS: DEGRADADO ({active}/3 servicios activos)";
        OverallStatusText.Foreground = !reachable ? Brush("#FF8181") : active == 3 ? Brush("#4ED68A") : Brush("#FFD166");
        DashboardSourceText.Text = canUseSsh
            ? "Fuente: SSH remoto (docker ps + /proc + df)"
            : reachable ? "Fuente: TCP local, configura credenciales para detalle" : "Fuente: sin conexion";
        LastCheckedText.Text = "Ultima verificacion: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        RefreshRecentRuns();
    }

    private void RefreshRecentRuns()
    {
        _recentRuns.Clear();
        foreach (var run in _state.GetRuns().OrderByDescending(r => r.EndedAt).Take(20))
        {
            _recentRuns.Add(new OperationRunRow(
                run.StartedAt.ToString("MM-dd HH:mm"),
                run.EndedAt.ToString("MM-dd HH:mm"),
                $"{run.Duration.TotalMinutes:F1}m",
                run.Action,
                run.Environment.ToString(),
                run.ExitCode,
                run.Succeeded ? "OK" : "FAIL"));
        }
    }

    private async void RefreshStorageButton_Click(object sender, RoutedEventArgs e) => await RefreshStorageAsync();
    private async void PruneBuilderButton_Click(object sender, RoutedEventArgs e) => await PruneAsync("builder");
    private async void PruneImagesButton_Click(object sender, RoutedEventArgs e) => await PruneAsync("images");

    private async void PruneAllButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Esto eliminara imagenes, contenedores detenidos y volumenes no usados. Continuar?",
            "Confirmacion",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
            await PruneAsync("system");
        }
    }

    private async Task RefreshStorageAsync()
    {
        if (!ValidateRemoteCredentials(false)) return;

        StorageStatusText.Text = "Consultando almacenamiento remoto...";
        StorageStatusText.Foreground = Brush("#78D6FF");

        var raw = await HealthCheckService.TryGetRemoteStorageInfoAsync(
            ServerHostText.Text.Trim(),
            ServerUserText.Text.Trim(),
            GetSshPort(),
            NullIfEmpty(SshKeyPathText.Text.Trim()),
            NullIfEmpty(SshPasswordBox.Password));

        if (string.IsNullOrWhiteSpace(raw))
        {
            StorageStatusText.Text = "No se pudo obtener datos del VPS.";
            StorageStatusText.Foreground = Brush("#FF8181");
            return;
        }

        ParseAndDisplayStorage(raw);
        StorageStatusText.Text = "Actualizado: " + DateTime.Now.ToString("HH:mm:ss");
        StorageStatusText.Foreground = Brush("#4ED68A");
    }

    private async Task PruneAsync(string target)
    {
        if (!ValidateRemoteCredentials(true)) return;

        StorageStatusText.Text = "Limpiando " + target + "...";
        StorageStatusText.Foreground = Brush("#FFD166");

        var result = await HealthCheckService.TryRunDockerPruneAsync(
            ServerHostText.Text.Trim(),
            ServerUserText.Text.Trim(),
            GetSshPort(),
            NullIfEmpty(SshKeyPathText.Text.Trim()),
            NullIfEmpty(SshPasswordBox.Password),
            target);

        if (!string.IsNullOrWhiteSpace(result))
        {
            MessageBox.Show(result, "Resultado prune", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        await RefreshStorageAsync();
    }

    private void ParseAndDisplayStorage(string raw)
    {
        var section = "";
        var imageTable = new DataTable();
        imageTable.Columns.Add("Imagen");
        imageTable.Columns.Add("Tamano");
        imageTable.Columns.Add("ID");

        var dirTable = new DataTable();
        dirTable.Columns.Add("Tamano");
        dirTable.Columns.Add("Directorio");

        foreach (var rawLine in raw.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var line = rawLine.Trim();
            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                section = line;
                continue;
            }

            switch (section)
            {
                case "[DISK]":
                    var disk = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                    if (disk.Length >= 5 && disk[0] != "Filesystem")
                    {
                        DiskUsedText.Text = disk[2];
                        DiskFreeText.Text = disk[3];
                        DiskPercentText.Text = disk[4];
                    }
                    break;
                case "[DOCKER_IMAGES]":
                    var image = line.Split('\t');
                    if (image.Length >= 3)
                    {
                        imageTable.Rows.Add(image[0], image[1], image[2]);
                    }
                    break;
                case "[TOP_DIRS]":
                    var dir = line.Split('\t');
                    if (dir.Length >= 2)
                    {
                        dirTable.Rows.Add(dir[0], dir[1]);
                    }
                    break;
            }
        }

        DockerImagesGrid.ItemsSource = imageTable.DefaultView;
        TopDirectoriesGrid.ItemsSource = dirTable.DefaultView;
    }

    private async void LoadRemoteLogButton_Click(object sender, RoutedEventArgs e)
    {
        if (!ValidateRemoteCredentials(true)) return;
        var source = GetComboText(RemoteLogSourceCombo);
        var tail = int.TryParse(RemoteLogTailText.Text, out var parsedTail) ? parsedTail : 300;

        RemoteLogTextBox.Text = "Cargando log remoto...";
        var log = await HealthCheckService.TryGetRemoteLogAsync(
            ServerHostText.Text.Trim(),
            ServerUserText.Text.Trim(),
            GetSshPort(),
            NullIfEmpty(SshKeyPathText.Text.Trim()),
            NullIfEmpty(SshPasswordBox.Password),
            source,
            tail);

        RemoteLogTextBox.Text = log ?? "No se pudo obtener el log remoto.";
    }

    private void ClearLogButton_Click(object sender, RoutedEventArgs e)
    {
        RemoteLogTextBox.Clear();
        LogTextBox.Clear();
    }

    private bool ValidateRemoteCredentials(bool showMessage)
    {
        var ok = !string.IsNullOrWhiteSpace(ServerHostText.Text) && !string.IsNullOrWhiteSpace(ServerUserText.Text);
        if (!ok && showMessage)
        {
            MessageBox.Show("Configura SERVER_HOST y SERVER_USER.", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        return ok;
    }

    private void OpenScriptsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var path = Path.Combine(RepoLocalText.Text.Trim(), "scripts");
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException("No se encontro la carpeta scripts: " + path);
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OpenLogButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!File.Exists(LogFilePath))
            {
                MessageBox.Show("Todavia no hay archivo de log generado.", "Log no encontrado", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Process.Start(new ProcessStartInfo(LogFilePath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void AdvancedToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _advancedVisible = !_advancedVisible;
        ApplyAdvancedVisibility();
    }

    private void ApplyAdvancedVisibility()
    {
        AdvancedGeneralPanel.Visibility = _advancedVisible ? Visibility.Visible : Visibility.Collapsed;
        AdvancedSshPanel.Visibility = _advancedVisible ? Visibility.Visible : Visibility.Collapsed;
        AdvancedSshExtrasPanel.Visibility = _advancedVisible ? Visibility.Visible : Visibility.Collapsed;
        AdvancedToggleButton.Content = _advancedVisible ? "[ SYS.ADVANCED: ON ]" : "[ SYS.ADVANCED ]";
    }

    private void EnvironmentButton_Click(object sender, RoutedEventArgs e)
    {
        _state.CurrentEnvironment = _state.CurrentEnvironment switch
        {
            DeploymentEnvironment.Development => DeploymentEnvironment.Staging,
            DeploymentEnvironment.Staging => DeploymentEnvironment.Production,
            _ => DeploymentEnvironment.Development
        };

        ApplyEnvironmentVisuals();
        SaveSettings();
        AppendLog("Objetivo de entorno configurado: " + _state.CurrentEnvironment, "ENV", false);
        _ = RefreshEnvironmentStatusAsync();
    }

    private void ApplyEnvironmentVisuals()
    {
        var profile = EnvironmentPolicy.GetProfile(_state.CurrentEnvironment);
        EnvironmentButton.Content = profile.RiskLabel;
        EnvironmentButton.Background = Brush(profile.RiskColor);
    }

    private void ReloadEnvButton_Click(object sender, RoutedEventArgs e) => LoadEnvVariables();
    private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSettings();
        UpdateSettingsSummary();
        AppendLog("Settings locales guardados.");
    }

    private void LoadSettings()
    {
        try
        {
            if (!File.Exists(SettingsFilePath)) return;
            var settings = JsonSerializer.Deserialize<UiSettings>(File.ReadAllText(SettingsFilePath));
            if (settings == null) return;

            RememberSshPasswordCheck.IsChecked = settings.RememberSshPassword;
            if (settings.RememberSshPassword && !string.IsNullOrWhiteSpace(settings.EncryptedSshPassword))
            {
                SshPasswordBox.Password = Unprotect(settings.EncryptedSshPassword);
            }

            if (!string.IsNullOrWhiteSpace(settings.Environment)
                && Enum.TryParse<DeploymentEnvironment>(settings.Environment, out var env))
            {
                _state.CurrentEnvironment = env;
            }
        }
        catch
        {
            // Ignorar settings corruptos.
        }
    }

    private void SaveSettings()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
            var settings = new UiSettings
            {
                RememberSshPassword = RememberSshPasswordCheck.IsChecked == true,
                EncryptedSshPassword = RememberSshPasswordCheck.IsChecked == true && !string.IsNullOrWhiteSpace(SshPasswordBox.Password)
                    ? Protect(SshPasswordBox.Password)
                    : null,
                Environment = _state.CurrentEnvironment.ToString()
            };

            File.WriteAllText(SettingsFilePath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch
        {
            // No bloquear ejecucion si AppData falla.
        }
    }

    private void UpdateSettingsSummary()
    {
        SettingsSummaryText.Text =
            $"Repo root: {RepositoryRoot}\n" +
            $"Repo local: {RepoLocalText.Text}\n" +
            $"Servidor: {ServerHostText.Text}:{GetSshPort()}\n" +
            $"Usuario: {ServerUserText.Text}\n" +
            $"Entorno objetivo: {_state.CurrentEnvironment}\n" +
            $"Log local: {LogFilePath}";
    }

    private void AppendLog(string message, string source = "UI", bool evaluatePolicy = true)
    {
        var now = DateTime.Now;
        var sanitized = LogSecurity.Sanitize(message);
        var line = $"[{now:HH:mm:ss}] {sanitized}";
        LogTextBox.AppendText(line + Environment.NewLine);
        LogTextBox.ScrollToEnd();

        _state.AddLog(new LogEntry(now, source, message, sanitized, LogClassifier.Classify(sanitized)));

        if (evaluatePolicy)
        {
            foreach (var result in EnvironmentPolicy.ValidateRuntimeLine(_state.CurrentEnvironment, sanitized))
            {
                _state.AddAlert(new AlertEvent(now, "EnvironmentPolicy", result.IsBlocking ? "Critical" : "Warning", result.Message, true));
                AppendLog($"POLICY {(result.IsBlocking ? "BLOCK" : "WARN")}: {result.Message}", "POLICY", false);
            }
        }

        try
        {
            File.AppendAllText(LogFilePath, line + Environment.NewLine, Encoding.UTF8);
        }
        catch
        {
            // No bloquear UI por permisos del log.
        }
    }

    private static void SetMetric(TextBlock label, IReadOnlyDictionary<string, string>? metrics, string key, string fallback)
    {
        label.Text = metrics != null && metrics.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : fallback;
    }

    private static void SetServiceLabel(TextBlock label, string state)
    {
        if (state == "running")
        {
            label.Text = "Servicio activo";
            label.Foreground = Brush("#4ED68A");
        }
        else if (state == "stopped")
        {
            label.Text = "Servicio detenido";
            label.Foreground = Brush("#FF8181");
        }
        else
        {
            label.Text = "Estado no verificado";
            label.Foreground = Brush("#FFD166");
        }
    }

    private string GetSelectedAction() => GetComboText(ActionCombo, "Deploy completo");
    private string GetSelectedSshAuth() => ParseOptionCode(GetComboText(SshAuthCombo), "Auto");
    private string GetSelectedDeployTarget() => ParseOptionCode(GetComboText(DeployTargetCombo), "Both");
    private string GetSelectedMigrationMode() => ParseOptionCode(GetComboText(MigrationModeCombo), "B");

    private string GetEffectiveSshAuthMode()
    {
        var selected = GetSelectedSshAuth();
        return selected.Equals("Auto", StringComparison.OrdinalIgnoreCase)
               && !string.IsNullOrWhiteSpace(SshPasswordBox.Password)
            ? "Password"
            : selected;
    }

    private int GetSshPort() => int.TryParse(SshPortText.Text.Trim(), out var port) ? port : 22;

    private static string GetComboText(ComboBox combo, string fallback = "")
    {
        return combo.SelectedItem is ComboBoxItem item
            ? item.Content?.ToString() ?? fallback
            : fallback;
    }

    private static string ParseOptionCode(string? value, string fallback)
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;
        var idx = value.IndexOf(" - ", StringComparison.Ordinal);
        return idx > 0 ? value[..idx].Trim() : value.Trim();
    }

    private static IEnumerable<string> RedactArgs(IEnumerable<string> args)
    {
        var redactNext = false;
        foreach (var arg in args)
        {
            if (redactNext)
            {
                yield return "***";
                redactNext = false;
                continue;
            }

            yield return arg;
            redactNext = arg is "-SshPassword" or "-GitToken";
        }
    }

    private static string QuoteForDisplay(string value) => value.Contains(' ') ? $"\"{value}\"" : value;
    private static string QuoteForCommandLine(string value) => "'" + value.Replace("'", "''") + "'";
    private static string? NullIfEmpty(string value) => string.IsNullOrWhiteSpace(value) ? null : value;

    private static Brush Brush(string color) => new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));

    private static string Protect(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser));
    }

    private static string Unprotect(string encryptedBase64)
    {
        var bytes = Convert.FromBase64String(encryptedBase64);
        return Encoding.UTF8.GetString(ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser));
    }

    private static string GetRuntimeModeLabel()
    {
#if DEBUG
        return "DEBUG";
#else
        return "PRODUCCION";
#endif
    }

    [GeneratedRegex(@"\x1b\[[0-9;]*[mGKHF]")]
    private static partial Regex AnsiRegex();

    private sealed class UiSettings
    {
        public bool RememberSshPassword { get; set; }
        public string? EncryptedSshPassword { get; set; }
        public string? Environment { get; set; }
    }

    public sealed record OperationRunRow(
        string Inicio,
        string Fin,
        string Duracion,
        string Accion,
        string Entorno,
        int Exit,
        string Resultado);
}
