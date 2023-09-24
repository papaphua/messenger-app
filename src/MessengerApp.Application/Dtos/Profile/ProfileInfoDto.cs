using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.Profile;

public sealed class ProfileInfoDto
{
    [Required(ErrorMessage = "UsernameRequired")]
    [DisplayName("Username")]
    public string UserName { get; set; } = null!;

    [DisplayName("FirstName")] public string? FirstName { get; set; }

    [DisplayName("LastName")] public string? LastName { get; set; }

    [DisplayName("Bio")] public string? Biography { get; set; }
}