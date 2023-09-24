using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Dtos.User;
using MessengerApp.Application.Helpers;
using MessengerApp.Application.Resources;
using MessengerApp.Domain.Enumerations;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Results = MessengerApp.Domain.Enumerations.Results;

namespace MessengerApp.Application.Services.SearchService;

public sealed class SearchService : ISearchService
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public SearchService(IDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<SearchDto>> SearchChatsAsync(string? search)
    {
        var users = await FindUsersByUsernameAsync(search);
        var groups = await FindGroupsByTitleAsync(search);
        var channels = await FindChannelsByTitleAsync(search);

        if (users.Count == 0 && groups.Count == 0 && channels.Count == 0)
            return new Result<SearchDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.NoSearchResultsFor, search)
            };

        var dto = new SearchDto
        {
            Users = users,
            Groups = groups,
            Channels = channels
        };

        return new Result<SearchDto> { Data = dto };
    }

    private async Task<IReadOnlyList<UserPreviewDto>> FindUsersByUsernameAsync(string? search)
    {
        var users = _dbContext.Set<User>()
            .Where(user => EF.Functions.Like(user.UserName!, $"%{search}%"));

        return await users.Select(user => _mapper.Map<UserPreviewDto>(user))
            .ToListAsync();
    }

    private async Task<IReadOnlyList<GroupPreviewDto>> FindGroupsByTitleAsync(string? search)
    {
        var groups = _dbContext.Set<Group>()
            .Where(group => EF.Functions.Like(group.Title, $"%{search}%")
                            && !group.IsPrivate);

        return await groups.Select(group => _mapper.Map<GroupPreviewDto>(group))
            .ToListAsync();
    }

    private async Task<IReadOnlyList<ChannelPreviewDto>> FindChannelsByTitleAsync(string? search)
    {
        var channels = _dbContext.Set<Channel>()
            .Where(channel => EF.Functions.Like(channel.Title, $"%{search}%")
                              && !channel.IsPrivate);

        return await channels.Select(group => _mapper.Map<ChannelPreviewDto>(group))
            .ToListAsync();
    }
}