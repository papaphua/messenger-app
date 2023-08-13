using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class UserEmailDto
{
    [Required]
    public string Email { get; set; }
}