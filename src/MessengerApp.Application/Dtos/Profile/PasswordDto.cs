using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos.Profile;

public sealed class PasswordDto
{
    [Required(ErrorMessage = "CurrentPasswordRequired")]
    [DisplayName("CurrentPassword")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "NewPasswordRequired")]
    [DisplayName("NewPassword")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "ConfirmPasswordRequired")]
    [DisplayName("ConfirmPassword")]
    [Compare(nameof(NewPassword), ErrorMessage = "PasswordNotMatch")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}