namespace IotDeviceClient;

public class MessageReceiver : IMessageReceiver
{
    public Task ReceivePayload(DevicePayload payload)
    {
        Console.WriteLine($"Device {payload.DeviceId} connected");
        return Task.CompletedTask;
    }

    public Task ReceiveDisconnection(string deviceId)
    {
        Console.WriteLine($"Device {deviceId} disconnected");
        return Task.CompletedTask;
    }
}
