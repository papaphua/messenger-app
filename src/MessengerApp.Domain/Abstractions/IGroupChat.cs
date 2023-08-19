using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions;

public interface IGroupChat
{
    string Title { get; set; }
    byte[]? GroupChatPicture { get; set; }
    ICollection<User> Admins { get; set; }
}