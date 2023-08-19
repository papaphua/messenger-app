using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions;

public abstract class Chat
{
    public ICollection<User> Users { get; set; } = null!;
    
    public ICollection<Message> Messages { get; set; } = null!;
}