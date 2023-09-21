using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class ChannelReaction
    : Reaction<Channel, ChannelMessage, ChannelAttachment, ChannelReaction>
{
}