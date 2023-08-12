using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Biography { get; set; }
}