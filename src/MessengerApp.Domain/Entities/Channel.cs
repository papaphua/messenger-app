using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Channel
    : Chat<Channel, ChannelMessage, ChannelMessageAttachment, ChannelMessageReaction>, IAdministratedChat
{
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }

    public User Owner { get; set; } = null!;

    public string OwnerId { get; set; } = null!;

    public ICollection<User> Admins { get; set; } = null!;
}