using MessengerApp.Application.Dtos;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.SearchService;

public interface ISearchService
{
    Task<Result<SearchDto>> SearchAsync(string? search);
}