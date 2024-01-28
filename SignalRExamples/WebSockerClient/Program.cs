using System.Net.WebSockets;
using System.Text;

string url = "wss://localhost:5001/devicesHub";

try
{
    ClientWebSocket ws = new();

    await ws.ConnectAsync(
        new Uri(url), CancellationToken.None);

    List<byte> handshake = new(
       Encoding.UTF8.GetBytes(@"{""protocol"":""json"", ""version"":1}"))
        {
            0x1e
        };

    await ws.SendAsync(
        new ArraySegment<byte>(
            [.. handshake]),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);

    Console.WriteLine("WebSockets connection established");
    await ReceiveAsync(ws);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return;
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static async Task ReceiveAsync(ClientWebSocket ws)
{
    byte[] buffer = new byte[4096];

    try
    {
        while (true)
        {
            WebSocketReceiveResult result = await ws.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await ws.CloseOutputAsync(
                    WebSocketCloseStatus.NormalClosure,
                    string.Empty,
                    CancellationToken.None);
                break;
            }
            else
            {
                Console.WriteLine(
                    Encoding.Default.GetString(
                        Decode(buffer)));
                buffer = new byte[4096];
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return;
    }
}

static byte[] Decode(byte[] packet)
{
    int i = packet.Length - 1;
    while (i >= 0 && packet[i] == 0)
    {
        --i;
    }

    byte[] temp = new byte[i + 1];
    Array.Copy(packet, temp, i + 1);
    return temp;
}