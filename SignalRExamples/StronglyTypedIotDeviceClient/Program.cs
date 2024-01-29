using IotDeviceClient;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

string url = "https://localhost:5001/devicesHub";

HubConnection hubConnection = new HubConnectionBuilder()
                         .WithUrl(url)
                         .Build();

IDevicesHub hubProxy = hubConnection.CreateHubProxy<IDevicesHub>();
hubConnection.Register<IMessageReceiver>(new MessageReceiver());

await hubConnection.StartAsync();

DevicePayload payload = new()
{
    DeviceId = Guid.NewGuid().ToString(),
    Version = "1.0.1"
};

await hubProxy.RegisterDevice(payload);

Console.WriteLine("Press any key to exit.");
Console.ReadKey();

await hubProxy.DisconnectDevice(payload.DeviceId);