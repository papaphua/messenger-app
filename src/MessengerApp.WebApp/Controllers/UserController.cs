using MessengerApp.Application.Dtos;
using MessengerApp.Application.Services.UserService;
using MessengerApp.Domain.Constants;
using MessengerApp.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Profile()
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var result = await _userService.GetUserAsync(userId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        if (!result.Succeeded) return RedirectToAction("Index", "Home");

        var user = result.Data;

        return View(user);
    }

    public async Task<IActionResult> UpdateProfile(UserProfileDto profileDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var user = await _userService.GetUserAsync(userId);
            return View("Profile", user.Data);
        }

        var result = await _userService.UpdateUserProfileAsync(userId, profileDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> ChangeEmail(UserEmailDto emailDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var user = await _userService.GetUserAsync(userId);
            return View("Profile", user.Data);
        }

        var result = await _userService.ChangeEmailAsync(userId, emailDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> ChangePassword(UserPasswordDto passwordDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var user = await _userService.GetUserAsync(userId);
            return View("Profile", user.Data);
        }

        var result = await _userService.ChangePasswordAsync(userId, passwordDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }
}