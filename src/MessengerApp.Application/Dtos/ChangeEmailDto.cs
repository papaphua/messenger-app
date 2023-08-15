using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class ChangeEmailDto
{
    [Required] public required string NewEmail { get; set; }

    [Required] public required string Token { get; set; }
}