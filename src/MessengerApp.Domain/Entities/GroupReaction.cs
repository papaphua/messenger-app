using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public class GroupReaction
    : Reaction<Group, GroupMessage, GroupAttachment, GroupReaction>
{
}