using DeviceManager.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DeviceManager.Workers;

public class DeviceCommandService(IHubContext<DevicesHub> hubContext) : BackgroundService
{
    private readonly IHubContext<DevicesHub> _hubContext = hubContext;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int cycles = 3;

        while (cycles > 0)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

            await _hubContext.Clients.All.SendAsync("ReceiveCommand",
                "READ_SENSOR_METRICS", stoppingToken);

            cycles--;
        }
    }
}