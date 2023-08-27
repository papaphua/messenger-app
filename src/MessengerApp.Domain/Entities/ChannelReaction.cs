using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class ChannelReaction
    : Reaction<Channel, ChannelMessage, ChannelAttachment, ChannelReaction>
{
}