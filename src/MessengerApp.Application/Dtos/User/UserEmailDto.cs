using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.User;

public sealed class UserEmailDto
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    public bool IsConfirmed { get; init; }
}