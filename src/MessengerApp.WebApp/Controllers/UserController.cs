using MessengerApp.Application.Dtos.User;
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
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _userService.GetUserProfileAsync(userId);

        if (!result.Succeeded)
        {
            TempData[Notifications.Message] = result.Message;
            TempData[Notifications.Succeeded] = result.Succeeded;

            return RedirectToAction("Index", "Home");
        }

        var user = result.Data;

        return View(user);
    }

    public async Task<IActionResult> UploadProfilePicture()
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (Request.Form.Files.Count == 0)
        {
            ModelState.AddModelError("file", "Please select a valid image file.");

            var userResult = await _userService.GetUserProfileAsync(userId);
            return View("Profile", userResult.Data);
        }

        var profilePicture = Request.Form.Files[0];

        using var memoryStream = new MemoryStream();

        await profilePicture.CopyToAsync(memoryStream);
        var profilePictureDto = new UserProfilePictureDto { ProfilePictureBytes = memoryStream.ToArray() };
        var uploadResult = await _userService.UploadProfilePictureAsync(userId, profilePictureDto);

        TempData[Notifications.Message] = uploadResult.Message;
        TempData[Notifications.Succeeded] = uploadResult.Succeeded;

        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> UpdateProfile(UserInfoDto infoDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var userResult = await _userService.GetUserProfileAsync(userId);
            return View("Profile", userResult.Data);
        }

        var result = await _userService.UpdateUserInfoAsync(userId, infoDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var userResult = await _userService.GetUserProfileAsync(userId);
            return View("Profile", userResult.Data);
        }

        var result = await _userService.ChangePasswordAsync(userId, passwordDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> RequestEmailConfirmation()
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var result = await _userService.RequestEmailConfirmationAsync(userId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> EmailConfirmation()
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var token = HttpContext.Request.Query["token"].First()!;

        var result = await _userService.ConfirmEmailAsync(userId, token);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> RequestEmailChange(UserEmailDto emailDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var result = await _userService.RequestEmailChangeAsync(userId, emailDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> EmailChange()
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var token = HttpContext.Request.Query["token"].First()!;

        var result = await _userService.ChangeEmailAsync(userId, token);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Profile");
    }
}