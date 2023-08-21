namespace MessengerApp.Domain.Entities.Joints;

public sealed class DirectUser
{
    public Guid DirectId { get; set; }

    public Guid UserId { get; set; }
}