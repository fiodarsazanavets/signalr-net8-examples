namespace IotDeviceClient;

public interface IMessageReceiver
{
    Task ReceivePayload(DevicePayload payload);
    Task ReceiveDisconnection(string deviceId);
}
