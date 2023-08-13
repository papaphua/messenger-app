using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class UserDto
{
    [Required] public UserEmailDto UserEmailDto { get; set; }

    [Required] public UserProfileDto UserProfileDto { get; set; }
}