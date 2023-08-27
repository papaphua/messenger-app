using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class Channel
    : Chat<Channel, ChannelMessage, ChannelAttachment, ChannelReaction>, IAdministratedChat
{
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }
}