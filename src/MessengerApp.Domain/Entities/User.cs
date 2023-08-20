using MessengerApp.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Domain.Entities;

public sealed class User : IdentityUser<Guid>, IEntity
{
    public ICollection<Channel> Channels = new List<Channel>();

    public ICollection<Direct> Directs = new List<Direct>();

    public ICollection<Group> Groups = new List<Group>();

    public byte[]? ProfilePicture { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Biography { get; set; }

    public string? RequestedEmail { get; set; }

    public bool IsExternal { get; set; }

    Guid IEntity.Id
    {
        get => Id;
        set => Id = value;
    }
}