using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class GroupMember : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string GroupId { get; set; } = null!;

    public string MemberId { get; set; } = null!;
    
    public bool IsOwner { get; set; }

    public bool IsAdmin { get; set; }

    public static GroupMember AddMemberToGroup(string groupId, string userId)
    {
        return new GroupMember
        {
            GroupId = groupId,
            MemberId = userId
        };
    }

    public static GroupMember AddAdminToGroup(string groupId, string userId)
    {
        return new GroupMember
        {
            GroupId = groupId,
            MemberId = userId,
            IsAdmin = true
        };
    }
    
    public static GroupMember AddOwnerToGroup(string groupId, string userId)
    {
        return new GroupMember
        {
            GroupId = groupId,
            MemberId = userId,
            IsOwner = true
        };
    }
}