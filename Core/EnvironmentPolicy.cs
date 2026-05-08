namespace HolosMigratorUI.Core;

public static class EnvironmentPolicy
{
    public static readonly EnvironmentProfile Development = new(
        DeploymentEnvironment.Development,
        RequireSqlEncryption: false,
        AllowTrustServerCertificate: true,
        RiskLabel: "DEV",
        RiskColor: "#00FFFF");

    public static readonly EnvironmentProfile Staging = new(
        DeploymentEnvironment.Staging,
        RequireSqlEncryption: true,
        AllowTrustServerCertificate: true,
        RiskLabel: "STAGING",
        RiskColor: "#FFD166");

    public static readonly EnvironmentProfile Production = new(
        DeploymentEnvironment.Production,
        RequireSqlEncryption: true,
        AllowTrustServerCertificate: false,
        RiskLabel: "PROD",
        RiskColor: "#FF4D4D");

    public static EnvironmentProfile GetProfile(DeploymentEnvironment env)
    {
        return env switch
        {
            DeploymentEnvironment.Development => Development,
            DeploymentEnvironment.Staging => Staging,
            _ => Production
        };
    }

    public static IReadOnlyList<PolicyValidationResult> ValidateRuntimeLine(DeploymentEnvironment env, string line)
    {
        var results = new List<PolicyValidationResult>();
        if (string.IsNullOrWhiteSpace(line))
        {
            return results;
        }

        if (env == DeploymentEnvironment.Production)
        {
            if (line.Contains("Encrypt=False", StringComparison.OrdinalIgnoreCase))
            {
                results.Add(new PolicyValidationResult(true, "En producción se detectó Encrypt=False en una cadena de conexión."));
            }

            if (line.Contains("TrustServerCertificate=True", StringComparison.OrdinalIgnoreCase))
            {
                results.Add(new PolicyValidationResult(true, "En producción se detectó TrustServerCertificate=True en una cadena de conexión."));
            }
        }

        if (env == DeploymentEnvironment.Staging && line.Contains("Encrypt=False", StringComparison.OrdinalIgnoreCase))
        {
            results.Add(new PolicyValidationResult(false, "En staging se detectó Encrypt=False; recomendado habilitar cifrado."));
        }

        return results;
    }
}