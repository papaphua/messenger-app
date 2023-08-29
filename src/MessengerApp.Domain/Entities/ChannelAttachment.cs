using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class ChannelAttachment
    : Attachment<Channel, ChannelMessage, ChannelAttachment, ChannelReaction>
{
}