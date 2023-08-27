using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class GroupMessage
    : Message<Group, GroupMessage, GroupAttachment, GroupReaction>
{
}