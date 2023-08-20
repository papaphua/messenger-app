using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Channel
    : Chat<Channel, ChannelMessage, ChannelMessageAttachment, ChannelMessageReaction>, IAdministratedChat
{
    public ICollection<User> Admins { get; set; } = new List<User>();

    public string? Description { get; set; }

    public byte[]? ChatPicture { get; set; }
}