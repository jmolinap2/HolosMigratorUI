namespace HolosMigratorUI.Core;

public enum DeploymentEnvironment
{
    Development,
    Staging,
    Production
}

public enum LogSeverity
{
    Info,
    Warning,
    Error,
    Success,
    Debug
}

public sealed record LogEntry(
    DateTime Timestamp,
    string Source,
    string RawLine,
    string Message,
    LogSeverity Severity);

public sealed record OperationRunSummary(
    DateTime StartedAt,
    DateTime EndedAt,
    int ExitCode,
    string Action,
    string DeployTarget,
    string MigrationMode,
    DeploymentEnvironment Environment)
{
    public TimeSpan Duration => EndedAt - StartedAt;
    public bool Succeeded => ExitCode == 0;
}

public sealed record HealthSnapshot(
    bool HostReachable,
    bool SqlRunning,
    bool ApiRunning,
    bool FrontRunning,
    DateTime CheckedAt);

public sealed record AlertEvent(
    DateTime CreatedAt,
    string Rule,
    string Severity,
    string Message,
    bool IsActive);

public sealed record OperationPreset(
    string Name,
    string Action,
    string DeployTarget,
    string MigrationMode,
    bool SkipPull,
    bool SkipMigrations,
    bool SkipBuild,
    bool SkipPublicChecks);

public sealed record EnvironmentProfile(
    DeploymentEnvironment Environment,
    bool RequireSqlEncryption,
    bool AllowTrustServerCertificate,
    string RiskLabel,
    string RiskColor);

public sealed record PolicyValidationResult(bool IsBlocking, string Message);