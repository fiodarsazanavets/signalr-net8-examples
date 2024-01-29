using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace StreamingDevicesManager.Hubs;

public class DevicesHub : Hub
{
    public async Task BroadcastInitializationSequence(IAsyncEnumerable<string> stream)
    {
        await foreach (var item in stream)
        {
            await Clients.All.SendAsync("ReceiveMessage", item);
        }
    }

    public async IAsyncEnumerable<string> RegisterDevice(
        string deviceName,
        [EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        yield return $"Request received from {deviceName} device";
        yield return $"Device registration started for the {deviceName} device";
        await Task.Delay(1000, cancellationToken);
        yield return $"Device registration completed for the {deviceName} device";
        yield return $"Device {deviceName} is ready";
    }

    public async IAsyncEnumerable<string> RestartSystem(
        [EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        yield return "System restart requested";
        yield return "System restarting...";
        await Task.Delay(1000, cancellationToken);
        yield return "System restart complete";
        yield return "System is fully operational";
    }
}
