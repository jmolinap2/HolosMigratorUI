namespace HolosMigratorUI.Core;

public static class LogClassifier
{
    public static LogSeverity Classify(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return LogSeverity.Debug;
        }

        if (line.Contains("[OK]", StringComparison.OrdinalIgnoreCase)
            || line.Contains("correctamente", StringComparison.OrdinalIgnoreCase)
            || line.Contains("EXIT código=0", StringComparison.OrdinalIgnoreCase))
        {
            return LogSeverity.Success;
        }

        if (line.Contains("ERR:", StringComparison.OrdinalIgnoreCase)
            || line.Contains("ERROR", StringComparison.OrdinalIgnoreCase)
            || line.Contains("Exception", StringComparison.OrdinalIgnoreCase)
            || line.Contains("FATAL", StringComparison.OrdinalIgnoreCase)
            || line.Contains("Unhandled", StringComparison.OrdinalIgnoreCase))
        {
            // Docker envía algunos eventos normales por stderr; tratarlos como warning si son de create/start/run.
            if (line.Contains("Container", StringComparison.OrdinalIgnoreCase)
                && (line.Contains("Created", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("Creating", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("Started", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("Running", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("Recreated", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("Recreate", StringComparison.OrdinalIgnoreCase)))
            {
                return LogSeverity.Warning;
            }

            return LogSeverity.Error;
        }

        if (line.Contains("warning", StringComparison.OrdinalIgnoreCase))
        {
            return LogSeverity.Warning;
        }

        return LogSeverity.Info;
    }
}