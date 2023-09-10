using Microsoft.AspNetCore.SignalR;

namespace MessengerApp.WebApp.Hubs;

public sealed class ChannelHub : Hub
{
    public async Task SendMessage(string username, string content, string timestamp,
        string profilePictureBytes, string channelId)
    {
        await Clients.Group(channelId)
            .SendAsync("ReceiveMessage", username, content, timestamp, profilePictureBytes);
    }

    public async Task JoinChannel(string channelId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
    }
}