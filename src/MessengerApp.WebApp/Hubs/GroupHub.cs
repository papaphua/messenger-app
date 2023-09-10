using Microsoft.AspNetCore.SignalR;

namespace MessengerApp.WebApp.Hubs;

public sealed class GroupHub : Hub
{
    public async Task SendMessage(string username, string content, string timestamp,
        string profilePictureBytes, string groupId)
    {
        await Clients.Group(groupId)
            .SendAsync("ReceiveMessage", username, content, timestamp, profilePictureBytes);
    }

    public async Task JoinGroup(string groupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }
}