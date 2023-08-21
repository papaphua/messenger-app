using MessengerApp.Application.Dtos.Direct;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.DirectService;

public interface IDirectService
{
    Task<Result<IEnumerable<DirectPreviewDto>>> GetDirectPreviews(string? userId);
    Task<Result> AddDirect(string? userId, string conversatorId);
    Task<Result> RemoveDirect(string? userId, Guid directId);
}