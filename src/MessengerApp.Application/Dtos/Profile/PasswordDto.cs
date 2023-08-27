using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.Profile;

public sealed class PasswordDto
{
    [Required]
    [DisplayName("Current password")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [DisplayName("New password")]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [DisplayName("Confirm new password")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}