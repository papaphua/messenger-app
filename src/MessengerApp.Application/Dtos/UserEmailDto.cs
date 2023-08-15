using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class UserEmailDto
{
    [Required] [EmailAddress] public required string Email { get; set; }
    public bool IsConfirmed { get; init; }
}