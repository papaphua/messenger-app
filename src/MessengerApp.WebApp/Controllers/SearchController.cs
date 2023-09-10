using MessengerApp.Application.Dtos;
using MessengerApp.Application.Services.ChannelService;
using MessengerApp.Application.Services.GroupService;
using MessengerApp.Application.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class SearchController : Controller
{
    private readonly IChannelService _channelService;
    private readonly IGroupService _groupService;
    private readonly IUserService _userService;

    public SearchController(IUserService userService, IGroupService groupService, IChannelService channelService)
    {
        _userService = userService;
        _groupService = groupService;
        _channelService = channelService;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var userResult = await _userService.FindUsersByUsernameAsync(search);
        var groupResult = await _groupService.FindGroupsByTitleAsync(search);
        var channelResult = await _channelService.FindChannelsByTitleAsync(search);

        return View(new SearchDto
        {
            Users = userResult.Data!,
            Groups = groupResult.Data!,
            Channels = channelResult.Data!
        });
    }
}