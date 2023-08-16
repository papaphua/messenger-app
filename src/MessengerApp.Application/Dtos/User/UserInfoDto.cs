using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.User;

public sealed class UserInfoDto
{
    [Required] [DisplayName("Username")] public required string UserName { get; set; }

    [DisplayName("First name")] public string? FirstName { get; set; }

    [DisplayName("Last name")] public string? LastName { get; set; }

    public string? Biography { get; set; }
}