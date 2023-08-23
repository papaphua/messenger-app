using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions;

public interface IAdministratedChat
{
    public ICollection<User> Admins { get; set; }

    public string? Description { get; set; }

    public byte[]? ChatPicture { get; set; }
}