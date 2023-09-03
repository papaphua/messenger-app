using Microsoft.AspNetCore.SignalR;

namespace MessengerApp.WebApp.Hubs;

public sealed class DirectHub : Hub
{
    public async Task SendMessage(string username, string content, string timestamp, string profilePictureBytes)
    {
        await Clients.All.SendAsync("ReceiveMessage", username, content, timestamp, profilePictureBytes);
    }
}