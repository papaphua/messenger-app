using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class ChannelMessageAttachment
    : Attachment<Channel, ChannelMessage, ChannelMessageAttachment, ChannelMessageReaction>
{
}