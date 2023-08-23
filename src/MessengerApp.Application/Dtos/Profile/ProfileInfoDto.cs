using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.Profile;

public sealed class ProfileInfoDto
{
    [Required] [DisplayName("Username")] public string UserName { get; set; } = null!;

    [DisplayName("First name")] public string? FirstName { get; set; }

    [DisplayName("Last name")] public string? LastName { get; set; }

    public string? Biography { get; set; }
}