using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class ChannelAttachment
    : Attachment<Channel, ChannelMessage, ChannelAttachment, ChannelReaction>
{
}