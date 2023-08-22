using MessengerApp.Application.Dtos.Direct;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.DirectService;

public interface IDirectService
{
    Task<Result<IEnumerable<DirectPreviewDto>>> GetDirectPreviewsAsync(string? userId);
    Task<Result> CreateDirectAsync(string? userId, string conversatorId);
    Task<Result> RemoveDirectAsync(string? userId, string directId);
}