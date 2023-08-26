using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessengerApp.Application.Dtos.Profile;

public sealed class ProfileEmailDto
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    [NotMapped] public bool IsConfirmed { get; init; }
}