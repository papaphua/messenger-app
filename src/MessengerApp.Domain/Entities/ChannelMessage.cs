using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class ChannelMessage
    : Message<Channel, ChannelMessage, ChannelMessageAttachment, ChannelMessageReaction>
{
}