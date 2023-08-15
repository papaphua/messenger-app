using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class UserProfileDto
{
    [Required] [DisplayName("Username")] public required string UserName { get; set; }

    [Required]
    [DisplayName("Profile picture")]
    public byte[]? ProfilePicture { get; set; }

    [DisplayName("First name")] public string? FirstName { get; set; }

    [DisplayName("Last name")] public string? LastName { get; set; }

    public string? Biography { get; set; }
}