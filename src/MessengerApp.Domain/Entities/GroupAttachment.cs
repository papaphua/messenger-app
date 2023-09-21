using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class GroupAttachment
    : Attachment<Group, GroupMessage, GroupAttachment, GroupReaction>
{
}