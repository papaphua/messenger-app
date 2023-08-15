using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class UserDto
{
    [Required] public required UserEmailDto UserEmailDto { get; set; }

    [Required] public required UserProfileDto UserProfileDto { get; set; }
}