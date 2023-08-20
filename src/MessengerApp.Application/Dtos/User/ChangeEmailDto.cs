using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.User;

public sealed class ChangeEmailDto
{
    [Required] [EmailAddress] public string NewEmail { get; set; } = null!;

    [Required] public string Token { get; set; } = null!;
}