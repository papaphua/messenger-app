using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class ChannelMessage
    : Message<Channel, ChannelMessage, ChannelAttachment, ChannelReaction>
{
    public ICollection<Comment> Comments { get; set; } = null!;
}