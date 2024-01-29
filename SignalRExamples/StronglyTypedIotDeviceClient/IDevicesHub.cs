namespace IotDeviceClient;

public interface IDevicesHub
{
    Task RegisterDevice(DevicePayload payload);
    Task DisconnectDevice(string deviceId);
}
