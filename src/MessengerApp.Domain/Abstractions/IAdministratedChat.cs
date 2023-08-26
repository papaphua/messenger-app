using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions;

public interface IAdministratedChat
{
    public string Title { get; set; }
    
    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }
    
    public User Owner { get; set; }
    
    public string OwnerId { get; set; }
    
    public ICollection<User> Admins { get; set; }
}