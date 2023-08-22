using MessengerApp.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Domain.Entities;

public sealed class User : IdentityUser, IEntity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Biography { get; set; }

    public byte[]? ProfilePictureBytes { get; set; }

    public string? RequestedEmail { get; set; }

    public bool IsExternal { get; set; }

    public ICollection<Direct> Directs { get; set; } = null!;

    public ICollection<Group> Groups { get; set; } = null!;

    public ICollection<Channel> Channels { get; set; } = null!;
}