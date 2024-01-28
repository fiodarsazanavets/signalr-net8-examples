namespace StronglyTypesChatApplication.Hubs;

public interface IChatHubClient
{
    Task ReceiveMessage(string message);
}
