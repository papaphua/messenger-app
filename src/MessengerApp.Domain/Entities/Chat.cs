using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Chat : Entity
{
    public Chat(Guid id) : base(id)
    {
    }

    public ICollection<User> Users { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = null!;
}