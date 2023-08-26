using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class GroupAdmin : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string GroupId { get; set; } = null!;

    public string AdminId { get; set; } = null!;
    
    public static GroupAdmin AddAdminToGroup(string groupId, string userId)
    {
        return new GroupAdmin
        {
            GroupId= groupId,
            AdminId = userId
        };
    }
}