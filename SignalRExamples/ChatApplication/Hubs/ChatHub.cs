﻿using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs;

public class ChatHub : Hub
{
    public async Task SendToEveryone(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    public async Task SendToOthers(string message)
    {
        await Clients.Others.SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    public async Task SendToCaller(string message)
    {
        await Clients.Caller.SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    public async Task SendToIndividual(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    public async Task SendToGroup(string groupName, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    public async Task SendToOthersInGroup(string groupName, string message)
    {
        await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    public async Task AddUserToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.SendAsync("ReceiveMessage", $"Current user added to {groupName} group");
        await Clients.Others.SendAsync("ReceiveMessage", $"User {Context.ConnectionId} added to {groupName} group");
    }

    public async Task RemoveUserFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.SendAsync("ReceiveMessage", $"Current user removed from {groupName} group");
        await Clients.Others.SendAsync("ReceiveMessage", $"User {Context.ConnectionId} removed from {groupName} group");
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "HubUsers");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "HubUsers");
        await base.OnDisconnectedAsync(exception);
    }

    private string GetMessageToSend(string originalMessage)
    {
        return $"User {Context.ConnectionId} said: {originalMessage}";
    }
}