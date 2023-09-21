using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class GroupMessage
    : Message<Group, GroupMessage, GroupAttachment, GroupReaction>
{
}