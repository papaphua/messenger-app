using MessengerApp.Application.Dtos.User;

namespace MessengerApp.Application.Dtos.Direct;

public sealed class DirectDto
{
    public Guid Id { get; set; }

    public UserInfoDto Conversator { get; set; } = null!;
}