using Microsoft.AspNetCore.SignalR;

namespace MessengerApp.WebApp.Hubs;

public sealed class DirectHub : Hub
{
    public async Task SendMessage(string username, string content, string timestamp, 
        string profilePictureBytes, string directId)
    {
        await Clients.Group(directId)
            .SendAsync("ReceiveMessage", username, content, timestamp, profilePictureBytes);
    }

    public async Task JoinDirect(string directId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, directId);
    }
}