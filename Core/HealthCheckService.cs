using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;

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
        int timeoutMs = 7000)
    {
        var output = await TryRunSshCommandAsync(
            host,
            user,
            port,
            keyPath,
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
        int timeoutMs = 9000)
    {
        var output = await TryRunSshCommandAsync(
            host,
            user,
            port,
            keyPath,
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

    private static async Task<string?> TryRunSshCommandAsync(
        string host,
        string user,
        int port,
        string? keyPath,
        string remoteCommand,
        int timeoutMs)
    {
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user))
        {
            return null;
        }

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo("ssh")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        process.StartInfo.ArgumentList.Add("-o");
        process.StartInfo.ArgumentList.Add("BatchMode=yes");
        process.StartInfo.ArgumentList.Add("-o");
        process.StartInfo.ArgumentList.Add("StrictHostKeyChecking=accept-new");
        process.StartInfo.ArgumentList.Add("-o");
        process.StartInfo.ArgumentList.Add($"ConnectTimeout={Math.Max(3, timeoutMs / 1000)}");
        process.StartInfo.ArgumentList.Add("-p");
        process.StartInfo.ArgumentList.Add(port.ToString());

        if (!string.IsNullOrWhiteSpace(keyPath))
        {
            process.StartInfo.ArgumentList.Add("-i");
            process.StartInfo.ArgumentList.Add(keyPath);
        }

        process.StartInfo.ArgumentList.Add($"{user}@{host}");
        process.StartInfo.ArgumentList.Add(remoteCommand);

        try
        {
            process.Start();
        }
        catch
        {
            return null;
        }

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        var waitTask = process.WaitForExitAsync();

        var completed = await Task.WhenAny(waitTask, Task.Delay(timeoutMs));
        if (completed != waitTask)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(true);
                }
            }
            catch
            {
                // Ignorar fallos al intentar detener el proceso.
            }

            return null;
        }

        var output = await outputTask;
        _ = await errorTask;

        return process.ExitCode == 0 ? output : null;
    }
}