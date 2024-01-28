namespace StronglyTypesChatApplication.Hubs;

public interface IChatHub
{
    Task SendToEveryone(string message);
    Task SendToOthers(string message);
    Task SendToCaller(string message);
    Task SendToIndividual(string connectionId, string message);
    Task SendToGroup(string groupName, string message);
    Task SendToOthersInGroup(string groupName, string message);
    Task AddUserToGroup(string groupName);
    Task RemoveUserFromGroup(string groupName);
}