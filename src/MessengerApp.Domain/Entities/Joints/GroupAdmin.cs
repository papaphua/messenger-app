using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class GroupAdmin : IEntity
{
    public Guid GroupId { get; set; }

    public Guid AdminId { get; set; }
    public Guid Id { get; set; }
}