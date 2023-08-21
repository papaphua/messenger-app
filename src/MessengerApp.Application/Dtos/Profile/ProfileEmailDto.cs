using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.Profile;

public sealed class ProfileEmailDto
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    public bool IsConfirmed { get; init; }
}