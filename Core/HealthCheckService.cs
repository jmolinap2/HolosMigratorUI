using System.Net.Sockets;
using System.Globalization;
using Renci.SshNet;

namespace HolosMigratorUI.Core;

public static class HealthCheckService
{
    public static async Task<bool> IsHostReachableAsync(string host, int port, int timeoutMs = 4000)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            return false;
        }

        try
        {
            using var client = new TcpClient();
            using var cts = new CancellationTokenSource(timeoutMs);
            await client.ConnectAsync(host, port, cts.Token);
            return client.Connected;
        }
        catch
        {
            return false;
        }
    }

    public static async Task<IReadOnlyDictionary<string, string>?> TryGetDockerServiceStatesAsync(
        string host,
        string user,
        int port,
        string? keyPath,
        string? password = null,
        int timeoutMs = 7000)
    {
        var output = await TryRunSshCommandAsync(
            host,
            user,
            port,
            keyPath,
            password,
            "docker ps --format '{{.Names}}|{{.Status}}'",
            timeoutMs);

        if (output == null)
        {
            return null;
        }

        var states = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var line = rawLine.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            var isRunning = line.Contains("|Up", StringComparison.OrdinalIgnoreCase);

            if (line.Contains("holos-sql", StringComparison.OrdinalIgnoreCase))
            {
                states["sql"] = isRunning ? "running" : "stopped";
            }

            if (line.Contains("holos-api", StringComparison.OrdinalIgnoreCase))
            {
                states["api"] = isRunning ? "running" : "stopped";
            }

            if (line.Contains("holos-front", StringComparison.OrdinalIgnoreCase))
            {
                states["front"] = isRunning ? "running" : "stopped";
            }
        }

        return states;
    }

    public static async Task<IReadOnlyDictionary<string, string>?> TryGetServerResourceMetricsAsync(
        string host,
        string user,
        int port,
        string? keyPath,
        string? password = null,
        int timeoutMs = 9000)
    {
        var output = await TryRunSshCommandAsync(
            host,
            user,
            port,
            keyPath,
            password,
            "bash -lc \"LC_ALL=C; cat /proc/loadavg; nproc; free -m; df -h /; uptime -p 2>/dev/null || uptime\"",
            timeoutMs);

        if (output == null)
        {
            return null;
        }

        var metrics = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var lines = output
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0)
            .ToList();

        if (lines.Count == 0)
        {
            return null;
        }

        // Línea 1: loadavg
        var loadTokens = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (loadTokens.Length > 0
            && double.TryParse(loadTokens[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var load1))
        {
            var cores = 0;
            if (lines.Count > 1)
            {
                _ = int.TryParse(lines[1], out cores);
            }

            if (cores > 0)
            {
                var cpuPercent = Math.Clamp((load1 / cores) * 100.0, 0.0, 100.0);
                metrics["cpu"] = $"{cpuPercent:F0}% (carga {load1:F2})";
            }
            else
            {
                metrics["cpu"] = $"Carga 1m: {load1:F2}";
            }
        }

        var memLine = lines.FirstOrDefault(l => l.StartsWith("Mem:", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(memLine))
        {
            var memParts = memLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (memParts.Length >= 3
                && long.TryParse(memParts[1], out var memTotal)
                && long.TryParse(memParts[2], out var memUsed)
                && memTotal > 0)
            {
                var memPct = (memUsed * 100.0) / memTotal;
                metrics["memory"] = $"{memUsed}/{memTotal} MB ({memPct:F0}%)";
            }
        }

        var diskLine = lines.FirstOrDefault(l => l.StartsWith("/", StringComparison.OrdinalIgnoreCase) && l.Contains('%'));
        if (!string.IsNullOrWhiteSpace(diskLine))
        {
            var diskParts = diskLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (diskParts.Length >= 5)
            {
                var diskTotal = diskParts[1];
                var diskUsed = diskParts[2];
                var diskPct = diskParts[4];
                metrics["disk"] = $"{diskUsed}/{diskTotal} ({diskPct})";
            }
        }

        var uptimeLine = lines.LastOrDefault(l => l.StartsWith("up ", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(uptimeLine))
        {
            metrics["uptime"] = CompactUptime(uptimeLine[3..].Trim());
        }

        return metrics;
    }

    public static Task<string?> TryGetRemoteEnvironmentSnapshotAsync(
        string host,
        string user,
        int port,
        string? keyPath,
        string? password = null,
        int timeoutMs = 12000)
    {
        const string remoteCommand =
            "bash -lc \"LC_ALL=C; " +
            "echo '[HOST]'; " +
            "printenv | egrep '^(ASPNETCORE_ENVIRONMENT|DOTNET_ENVIRONMENT|NODE_ENV|HOLOS_|TZ|SQL|ConnectionStrings__)=' | sort || true; " +
            "for pair in 'holos-api:holos-api|holos_api|api' 'holos-front:holos-front|holos_front|front' 'holos-sql:holos-sql|holos_sql|sql'; do " +
            "c=${pair%%:*}; regex=${pair#*:}; " +
            "resolved=$(docker ps -a --format '{{.Names}}' | grep -Ei -m1 \"$regex\" || true); " +
            "if [ -n \"$resolved\" ]; then " +
            "echo; echo \"[CONTAINER:$c]\"; " +
            "docker inspect -f '{{range .Config.Env}}{{println .}}{{end}}' \"$resolved\" " +
            "| egrep '^(ASPNETCORE_ENVIRONMENT|DOTNET_ENVIRONMENT|NODE_ENV|HOLOS_|TZ|SQL|ConnectionStrings__)=' | sort || true; " +
            "fi; done\"";

        return TryRunSshCommandAsync(host, user, port, keyPath, password, remoteCommand, timeoutMs);
    }

    public static Task<string?> TryGetRemoteStorageInfoAsync(
        string host, string user, int port, string? keyPath, string? password = null, int timeoutMs = 14000)
    {
        const string cmd =
            "bash -lc \"LC_ALL=C; " +
            "echo '[DISK]'; df -h / 2>/dev/null; " +
            "echo '[DOCKER]'; docker system df 2>/dev/null; " +
            "echo '[DOCKER_IMAGES]'; docker images --format '{{.Repository}}:{{.Tag}}\\t{{.Size}}\\t{{.ID}}' 2>/dev/null; " +
            "echo '[TOP_DIRS]'; du -xh / --max-depth=2 2>/dev/null | sort -h | tail -n 20\"";

        return TryRunSshCommandAsync(host, user, port, keyPath, password, cmd, timeoutMs);
    }

    public static Task<string?> TryRunDockerPruneAsync(
        string host, string user, int port, string? keyPath, string? password = null, string target = "builder", int timeoutMs = 60000)
    {
        var cmd = target switch
        {
            "builder" => "bash -lc \"docker builder prune -af 2>&1 | tail -5\"",
            "images"  => "bash -lc \"docker image prune -af 2>&1 | tail -5\"",
            "system"  => "bash -lc \"docker system prune -af --volumes 2>&1 | tail -5\"",
            _         => "echo 'target desconocido'"
        };

        return TryRunSshCommandAsync(host, user, port, keyPath, password, cmd, timeoutMs);
    }

    public static Task<string?> TryGetRemoteLogAsync(
        string host,
        string user,
        int port,
        string? keyPath,
        string? password,
        string logSource,
        int tailLines = 300,
        int timeoutMs = 12000)
    {
        var safeTail = Math.Clamp(tailLines, 50, 5000);

        var remoteCommand = logSource switch
        {
            "host-syslog" => $"bash -lc \"tail -n {safeTail} /var/log/syslog 2>/dev/null || journalctl -n {safeTail} --no-pager\"",
            "host-auth" => $"bash -lc \"tail -n {safeTail} /var/log/auth.log 2>/dev/null || journalctl -u ssh -n {safeTail} --no-pager\"",
            "migrator" => $"bash -lc \"tail -n {safeTail} /root/OmniSuite/migrator_logs.txt 2>/dev/null || echo '__LOG_NOT_FOUND__ /root/OmniSuite/migrator_logs.txt'\"",
            "holos-api" or "holos-front" or "holos-sql" => $"docker logs --tail {safeTail} {logSource} 2>&1",
            _ => null
        };

        if (remoteCommand == null)
        {
            return Task.FromResult<string?>(null);
        }

        return TryRunSshCommandAsync(host, user, port, keyPath, password, remoteCommand, timeoutMs);
    }

    private static Task<string?> TryRunSshCommandAsync(
        string host,
        string user,
        int port,
        string? keyPath,
        string? password,
        string remoteCommand,
        int timeoutMs)
    {
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user))
        {
            return Task.FromResult<string?>(null);
        }

        AuthenticationMethod? keyAuth = null;
        AuthenticationMethod? passAuth = null;

        if (!string.IsNullOrWhiteSpace(keyPath) && File.Exists(keyPath))
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(password))
                {
                    keyAuth = new PrivateKeyAuthenticationMethod(user, new PrivateKeyFile(keyPath, password));
                }
                else
                {
                    keyAuth = new PrivateKeyAuthenticationMethod(user, new PrivateKeyFile(keyPath));
                }
            }
            catch
            {
                try
                {
                    keyAuth = new PrivateKeyAuthenticationMethod(user, new PrivateKeyFile(keyPath));
                }
                catch
                {
                    keyAuth = null;
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(password))
        {
            passAuth = new PasswordAuthenticationMethod(user, password);
        }

        var methods = new List<AuthenticationMethod>();
        if (keyAuth != null) methods.Add(keyAuth);
        if (passAuth != null) methods.Add(passAuth);

        if (methods.Count == 0)
        {
            return Task.FromResult<string?>(null);
        }

        var connectionInfo = new ConnectionInfo(host, port, user, [.. methods])
        {
            Timeout = TimeSpan.FromMilliseconds(timeoutMs)
        };

        return Task.Run(() =>
        {
            try
            {
                using var client = new SshClient(connectionInfo);
                client.Connect();
                using var cmd = client.CreateCommand(remoteCommand);
                cmd.CommandTimeout = TimeSpan.FromMilliseconds(timeoutMs);
                var result = cmd.Execute();
                client.Disconnect();
                return cmd.ExitStatus == 0 ? result : null;
            }
            catch
            {
                return null;
            }
        });
    }

    // Convierte "2 weeks, 2 days, 1 hour, 10 minutes" → "2w 2d 1h 10m"
    private static string CompactUptime(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return raw;

        var map = new (string[] keys, string suffix)[]
        {
            (["weeks", "week"],   "w"),
            (["days",  "day"],    "d"),
            (["hours", "hour"],   "h"),
            (["minutes","minute"],"m"),
        };

        var parts = new List<string>();
        foreach (var (keys, suffix) in map)
        {
            foreach (var key in keys)
            {
                var idx = raw.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                if (idx <= 0) continue;
                var numStart = idx - 1;
                while (numStart > 0 && char.IsDigit(raw[numStart - 1])) numStart--;
                if (int.TryParse(raw[numStart..idx].Trim(), out int n))
                    parts.Add($"{n}{suffix}");
                break;
            }
        }

        return parts.Count > 0 ? string.Join(" ", parts) : raw;
    }
}