using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Chat : IEntity
{
    public ICollection<User> Users { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = null!;
    public Guid Id { get; set; }
}