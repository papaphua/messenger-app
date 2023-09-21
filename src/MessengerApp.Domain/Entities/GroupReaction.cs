using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public class GroupReaction
    : Reaction<Group, GroupMessage, GroupAttachment, GroupReaction>
{
}