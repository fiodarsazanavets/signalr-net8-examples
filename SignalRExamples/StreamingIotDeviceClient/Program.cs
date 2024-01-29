using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Channels;

string url = "https://localhost:5001/devicesHub";
string deviceName = "Device1";

HubConnection hubConnection = new HubConnectionBuilder()
                         .WithUrl(url)
                         .Build();

await hubConnection.StartAsync();

Channel<string> channel = Channel.CreateUnbounded<string>();
await hubConnection.SendAsync(
    "BroadcastInitializationSequence", channel.Reader);

await channel.Writer.WriteAsync($"Device {deviceName} started");
await channel.Writer.WriteAsync($"Device {deviceName} is loading...");
await channel.Writer.WriteAsync($"Device {deviceName} loaded successfully");
await channel.Writer.WriteAsync($"Device {deviceName} registration started");

CancellationTokenSource cancellationTokenSource = new();
IAsyncEnumerable<string> stream = hubConnection
    .StreamAsync<string>("RegisterDevice",
    deviceName, cancellationTokenSource.Token);

await foreach (var reply in stream)
{
    Console.WriteLine(reply);
}


await channel.Writer.WriteAsync($"Device {deviceName} registered successfully");
channel.Writer.Complete();

Console.WriteLine("Press any key to exit.");
Console.ReadKey();