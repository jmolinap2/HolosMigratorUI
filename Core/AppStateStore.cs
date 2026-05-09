using System.Collections.Concurrent;

namespace HolosMigratorUI.Core;

public sealed class AppStateStore
{
    private readonly object _sync = new();
    private readonly List<LogEntry> _logs = new();
    private readonly List<OperationRunSummary> _runs = new();
    private readonly List<AlertEvent> _alerts = new();
    private readonly ConcurrentDictionary<string, string> _serviceState = new(StringComparer.OrdinalIgnoreCase);

    public static AppStateStore Instance { get; } = new();

    public DeploymentEnvironment CurrentEnvironment { get; set; } = DeploymentEnvironment.Staging;

    public IReadOnlyList<OperationPreset> Presets =>
    [
        new OperationPreset("Deploy completo estándar", "Deploy completo", "Both", "B", false, false, false, false),
        new OperationPreset("Solo migraciones host+tenants", "Solo migraciones", "Both", "B", false, false, false, false),
        new OperationPreset("Deploy rápido sin checks", "Deploy completo", "Both", "N", false, true, false, true)
    ];

    public void AddLog(LogEntry entry)
    {
        lock (_sync)
        {
            _logs.Add(entry);
            if (_logs.Count > 10000)
            {
                _logs.RemoveRange(0, 2000);
            }
        }

        UpdateServiceState(entry.Message);
    }

    public void AddRun(OperationRunSummary summary)
    {
        lock (_sync)
        {
            _runs.Add(summary);
            if (_runs.Count > 1000)
            {
                _runs.RemoveRange(0, 200);
            }
        }

        EvaluateAlerts();
    }

    public void AddAlert(AlertEvent alert)
    {
        lock (_sync)
        {
            _alerts.Add(alert);
            if (_alerts.Count > 1000)
            {
                _alerts.RemoveRange(0, 200);
            }
        }
    }

    public IReadOnlyList<LogEntry> GetLogs()
    {
        lock (_sync)
        {
            return _logs.ToList();
        }
    }

    public IReadOnlyList<OperationRunSummary> GetRuns()
    {
        lock (_sync)
        {
            return _runs.ToList();
        }
    }

    public IReadOnlyList<AlertEvent> GetAlerts(bool onlyActive)
    {
        lock (_sync)
        {
            return onlyActive ? _alerts.Where(a => a.IsActive).ToList() : _alerts.ToList();
        }
    }

    public string GetServiceState(string service)
    {
        return _serviceState.TryGetValue(service, out var state) ? state : "unknown";
    }

    public void SetServiceState(string service, string state)
    {
        _serviceState[service] = state;
    }

    public HealthSnapshot GetLatestHealthSnapshot(string host)
    {
        var hostReachable = !_logs.Where(l => l.Timestamp > DateTime.Now.AddMinutes(-30))
            .Any(l => l.Message.Contains("host unreachable", StringComparison.OrdinalIgnoreCase));

        return new HealthSnapshot(
            HostReachable: hostReachable,
            SqlRunning: IsServiceRunning("sql"),
            ApiRunning: IsServiceRunning("api"),
            FrontRunning: IsServiceRunning("front"),
            CheckedAt: DateTime.Now);
    }

    public (double successRate7d, double successRate30d, double mttrMinutes, double avgDurationMinutes) GetKpis()
    {
        var now = DateTime.Now;
        List<OperationRunSummary> runs;
        lock (_sync)
        {
            runs = _runs.ToList();
        }

        var runs7 = runs.Where(r => r.EndedAt >= now.AddDays(-7)).ToList();
        var runs30 = runs.Where(r => r.EndedAt >= now.AddDays(-30)).ToList();

        var success7 = runs7.Count == 0 ? 0 : runs7.Count(r => r.Succeeded) * 100.0 / runs7.Count;
        var success30 = runs30.Count == 0 ? 0 : runs30.Count(r => r.Succeeded) * 100.0 / runs30.Count;

        var avgDuration = runs30.Count == 0 ? 0 : runs30.Average(r => r.Duration.TotalMinutes);

        // MTTR simplificado: tiempo promedio entre fallo y siguiente éxito.
        var mttrValues = new List<double>();
        var ordered = runs30.OrderBy(r => r.EndedAt).ToList();
        for (var i = 0; i < ordered.Count; i++)
        {
            if (ordered[i].Succeeded)
            {
                continue;
            }

            var nextSuccess = ordered.Skip(i + 1).FirstOrDefault(r => r.Succeeded);
            if (nextSuccess != null)
            {
                mttrValues.Add((nextSuccess.EndedAt - ordered[i].EndedAt).TotalMinutes);
            }
        }

        var mttr = mttrValues.Count == 0 ? 0 : mttrValues.Average();

        return (Math.Round(success7, 2), Math.Round(success30, 2), Math.Round(mttr, 2), Math.Round(avgDuration, 2));
    }

    private bool IsServiceRunning(string service)
    {
        return _serviceState.TryGetValue(service, out var state) && state == "running";
    }

    private void UpdateServiceState(string line)
    {
        void Mark(string key, string status)
        {
            _serviceState[key] = status;
        }

        if (line.Contains("holos-sql Running", StringComparison.OrdinalIgnoreCase))
        {
            Mark("sql", "running");
        }
        if (line.Contains("holos-api Started", StringComparison.OrdinalIgnoreCase))
        {
            Mark("api", "running");
        }
        if (line.Contains("holos-front Started", StringComparison.OrdinalIgnoreCase))
        {
            Mark("front", "running");
        }

        if (line.Contains("holos-api", StringComparison.OrdinalIgnoreCase) && line.Contains("Stopped", StringComparison.OrdinalIgnoreCase))
        {
            Mark("api", "stopped");
        }
        if (line.Contains("holos-front", StringComparison.OrdinalIgnoreCase) && line.Contains("Stopped", StringComparison.OrdinalIgnoreCase))
        {
            Mark("front", "stopped");
        }
        if (line.Contains("holos-sql", StringComparison.OrdinalIgnoreCase) && line.Contains("Stopped", StringComparison.OrdinalIgnoreCase))
        {
            Mark("sql", "stopped");
        }
    }

    private void EvaluateAlerts()
    {
        List<OperationRunSummary> ordered;
        lock (_sync)
        {
            ordered = _runs.OrderByDescending(r => r.EndedAt).Take(5).ToList();
        }

        var consecutiveFailures = ordered.TakeWhile(r => !r.Succeeded).Count();
        if (consecutiveFailures >= 2)
        {
            AddAlert(new AlertEvent(DateTime.Now, "ConsecutiveRunFailure", "High", $"Se detectaron {consecutiveFailures} ejecuciones fallidas consecutivas.", true));
        }

        if (_serviceState.TryGetValue("api", out var apiState) && apiState != "running")
        {
            AddAlert(new AlertEvent(DateTime.Now, "ApiDown", "High", "API no está en estado running.", true));
        }

        if (_serviceState.TryGetValue("sql", out var sqlState) && sqlState != "running")
        {
            AddAlert(new AlertEvent(DateTime.Now, "SqlDown", "High", "SQL no está en estado running.", true));
        }
    }
}