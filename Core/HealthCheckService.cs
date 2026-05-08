using System.Net.Sockets;

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
}