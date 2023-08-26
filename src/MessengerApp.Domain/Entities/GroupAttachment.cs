using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class GroupAttachment
    : Attachment<Group, GroupMessage, GroupAttachment, GroupReaction>
{
}