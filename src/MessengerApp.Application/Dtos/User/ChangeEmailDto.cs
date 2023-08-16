using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.User;

public sealed class ChangeEmailDto
{
    [Required] [EmailAddress] public required string NewEmail { get; set; }

    [Required] public required string Token { get; set; }
}