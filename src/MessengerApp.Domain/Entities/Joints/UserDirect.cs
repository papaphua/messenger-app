namespace MessengerApp.Domain.Entities.Joints;

public sealed class UserDirect
{
    public Guid UserId { get; set; }

    public Guid DirectId { get; set; }
}