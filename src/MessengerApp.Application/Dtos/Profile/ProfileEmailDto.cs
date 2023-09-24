using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessengerApp.Application.Dtos.Profile;

public sealed class ProfileEmailDto
{
    [Required(ErrorMessage = "EmailRequired")]
    [DisplayName("Email")]
    [EmailAddress(ErrorMessage = "NotEmail")]
    public string Email { get; set; } = null!;

    [NotMapped] public bool IsConfirmed { get; init; }
}