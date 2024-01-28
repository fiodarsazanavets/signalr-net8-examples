using IotDeviceClient;
using Microsoft.AspNetCore.SignalR.Client;

string url = "https://localhost:5001/devicesHub";

HubConnection hubConnection = new HubConnectionBuilder()
                         .WithUrl(url)
                         .Build();

hubConnection.On<DevicePayload>("ReceivePayload",
    payload => Console.WriteLine($"Device {payload.DeviceId} connected"));

hubConnection.On<string>("ReceiveDisconnection",
    deviceId => Console.WriteLine($"Device {deviceId} disconnected"));

await hubConnection.StartAsync();

DevicePayload payload = new()
{
    DeviceId = Guid.NewGuid().ToString(),
    Version = "1.0.1"
};

await hubConnection.InvokeAsync("RegisterDevice", payload);

Console.WriteLine("Press any key to exit.");
Console.ReadKey();

await hubConnection.InvokeAsync("DisconnectDevice", payload.DeviceId);