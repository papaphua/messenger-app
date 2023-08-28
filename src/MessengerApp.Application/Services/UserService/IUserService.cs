using MessengerApp.Application.Dtos.User;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.UserService;

public interface IUserService
{
    public Task<Result<IEnumerable<UserPreviewDto>>> SearchUsersByUsernameAsync(string? search);
}