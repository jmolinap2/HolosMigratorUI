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
            metrics["uptime"] = uptimeLine[3..].Trim();
        }

        return metrics;
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
                keyAuth = new PrivateKeyAuthenticationMethod(user, new PrivateKeyFile(keyPath));
            }
            catch
            {
                keyAuth = null;
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
}