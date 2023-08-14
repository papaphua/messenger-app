using Microsoft.AspNetCore.Identity;

namespace MessengerApp.IdentityServer.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public new string Email { get; set; }
    
    public new string UserName { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Biography { get; set; }

    public string RequestedEmail { get; set; }

    public bool IsExternal { get; set; } = false;
}