using System.Text.RegularExpressions;

namespace HolosMigratorUI.Core;

public static partial class LogSecurity
{
    public static string Sanitize(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return line;
        }

        var sanitized = line;

        // Common secret key/value pairs
        sanitized = SecretKvRegex().Replace(sanitized, m => $"{m.Groups[1].Value}=***");

        // Bearer-like and GitHub tokens
        sanitized = GitHubTokenRegex().Replace(sanitized, "ghp_***");
        sanitized = BearerRegex().Replace(sanitized, "Bearer ***");

        // Mask connection string credentials while preserving diagnostics context.
        sanitized = ConnectionPasswordRegex().Replace(sanitized, "Password=***;");
        sanitized = ConnectionUserRegex().Replace(sanitized, "User Id=***;");

        // Mask explicit command arguments that can include passwords.
        sanitized = SshPasswordArgRegex().Replace(sanitized, "-SshPassword ***");
        sanitized = GitTokenArgRegex().Replace(sanitized, "-GitToken ***");

        return sanitized;
    }

    [GeneratedRegex(@"(?i)\b(password|pwd|token|apikey|api_key|secret)\s*[:=]\s*([^;\s]+)")]
    private static partial Regex SecretKvRegex();

    [GeneratedRegex(@"\bghp_[A-Za-z0-9]{10,}\b")]
    private static partial Regex GitHubTokenRegex();

    [GeneratedRegex(@"(?i)Bearer\s+[A-Za-z0-9\-_\.]+")]
    private static partial Regex BearerRegex();

    [GeneratedRegex(@"(?i)Password\s*=\s*[^;]*;")]
    private static partial Regex ConnectionPasswordRegex();

    [GeneratedRegex(@"(?i)User\s*Id\s*=\s*[^;]*;")]
    private static partial Regex ConnectionUserRegex();

    [GeneratedRegex(@"(?i)-SshPassword\s+([^\s]+)")]
    private static partial Regex SshPasswordArgRegex();

    [GeneratedRegex(@"(?i)-GitToken\s+([^\s]+)")]
    private static partial Regex GitTokenArgRegex();
}