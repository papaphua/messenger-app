using Microsoft.AspNetCore.Identity;

namespace MessengerApp.IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public bool IsExternal { get; set; }
}