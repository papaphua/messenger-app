using MessengerApp.Application.Services.UserService;
using MessengerApp.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var result = await _userService.SearchUsersAsync(search);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        var userPreviews = result.Data;

        return View(userPreviews);
    }
}