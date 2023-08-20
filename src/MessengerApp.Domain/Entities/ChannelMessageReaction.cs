using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class ChannelMessageReaction
    : Reaction<Channel, ChannelMessage, ChannelMessageAttachment, ChannelMessageReaction>
{
}