using DeviceManager.Dto;
using Microsoft.AspNetCore.SignalR;

namespace DeviceManager.Hubs;

public class DevicesHub : Hub
{
    public async Task RegisterDevice(DevicePayload payload)
    {
        await Clients.All.SendAsync("ReceivePayload", payload);
    }

    public async Task DisconnectDevice(string deviceId)
    {
        await Clients.All.SendAsync("ReceiveDisconnection", deviceId);
    }
}