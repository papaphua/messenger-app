using MessengerApp.Application.Dtos.Profile;
using MessengerApp.Application.Services.ProfileService;
using MessengerApp.Domain.Constants;
using MessengerApp.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _profileService.GetProfileAsync(userId);

        if (result.Succeeded) return View(result.Data);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> UpdateProfileInfo(ProfileInfoDto profileInfoDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        if (!ModelState.IsValid)
        {
            var profile = (await _profileService.GetProfileAsync(userId)).Data;
            return View("Index", profile);
        }

        var result = await _profileService.UpdateProfileInfoAsync(userId, profileInfoDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Profile");
    }

    public async Task<IActionResult> UpdateProfilePicture()
    {
        var userId = Parser.ParseUserId(HttpContext);

        if (Request.Form.Files.Count == 0)
        {
            ModelState.AddModelError("file", "Please select a valid image file.");

            var profile = (await _profileService.GetProfileAsync(userId)).Data;
            return View("Index", profile);
        }

        var file = Request.Form.Files[0];

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var profilePictureBytes = memoryStream.ToArray();

        var result = await _profileService.UpdateProfilePictureAsync(userId, profilePictureBytes);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Profile");
    }

    public async Task<IActionResult> ChangePassword(PasswordDto passwordDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        if (!ModelState.IsValid)
        {
            var profile = (await _profileService.GetProfileAsync(userId)).Data;
            return View("Index", profile);
        }

        var result = await _profileService.ChangePasswordAsync(userId, passwordDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Profile");
    }

    public async Task<IActionResult> RequestEmailConfirmation()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _profileService.RequestEmailConfirmationAsync(userId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Profile");
    }

    public async Task<IActionResult> ConfirmEmail()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var token = HttpContext.Request.Query["token"].FirstOrDefault();

        var result = await _profileService.ConfirmEmailAsync(userId, token);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Profile");
    }

    public async Task<IActionResult> RequestEmailChange(ProfileEmailDto profileEmailDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        if (!ModelState.IsValid)
        {
            var profile = (await _profileService.GetProfileAsync(userId)).Data;
            return View("Index", profile);
        }

        var result = await _profileService.RequestEmailChangeAsync(userId, profileEmailDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Profile");
    }

    public async Task<IActionResult> ChangeEmail()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var token = HttpContext.Request.Query["token"].FirstOrDefault();

        var result = await _profileService.ChangeEmailAsync(userId, token);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Profile");
    }
}