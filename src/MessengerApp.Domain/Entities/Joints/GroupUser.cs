using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class GroupUser : IEntity
{
    public Guid GroupId { get; set; }

    public Guid UserId { get; set; }
    
    public Guid Id { get; set; }
}