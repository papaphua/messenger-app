using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class DirectUser : IEntity
{
    public Guid DirectId { get; set; }

    public Guid UserId { get; set; }
    public Guid Id { get; set; }
}