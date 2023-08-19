using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class GroupChat : Chat, IGroupChat, IEntity
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public byte[]? GroupChatPicture { get; set; }

    public ICollection<User> Admins { get; set; } = null!;
}