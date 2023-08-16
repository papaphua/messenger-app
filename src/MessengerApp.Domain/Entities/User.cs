using MessengerApp.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Domain.Entities;

public sealed class User : IdentityUser<Guid>, IEntity
{
    public new required string UserName { get; set; }
    
    public new required string Email { get; set; }

    public byte[]? ProfilePicture { get; set; }
    
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Biography { get; set; }

    public string? RequestedEmail { get; set; }

    public bool IsExternal { get; set; }

    public ICollection<Chat> Chats { get; set; } = null!;

    Guid IEntity.Id
    {
        get => Id;
        set => Id = value;
    }
}