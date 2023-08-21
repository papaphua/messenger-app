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

            var userResult = await _profileService.GetProfileAsync(userId);
            return View("Index", userResult.Data);
        }

        var profilePicture = Request.Form.Files[0];

        using var memoryStream = new MemoryStream();

        await profilePicture.CopyToAsync(memoryStream);
        var profilePictureDto = new ProfilePictureDto { ProfilePictureBytes = memoryStream.ToArray() };
        var uploadResult = await _profileService.UploadProfilePictureAsync(userId, profilePictureDto);

        TempData[Notifications.Message] = uploadResult.Message;
        TempData[Notifications.Succeeded] = uploadResult.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateProfile(ProfileInfoDto infoDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var userResult = await _profileService.GetProfileAsync(userId);
            return View("Index", userResult.Data);
        }

        var result = await _profileService.UpdateUserInfoAsync(userId, infoDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var userResult = await _profileService.GetProfileAsync(userId);
            return View("Index", userResult.Data);
        }

        var result = await _profileService.ChangePasswordAsync(userId, passwordDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RequestEmailConfirmation()
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var result = await _profileService.RequestEmailConfirmationAsync(userId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ConfirmEmail()
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var token = HttpContext.Request.Query["token"].First()!;

        var result = await _profileService.ConfirmEmailAsync(userId, token);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RequestEmailChange(ProfileEmailDto emailDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var result = await _profileService.RequestEmailChangeAsync(userId, emailDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ChangeEmail()
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var token = HttpContext.Request.Query["token"].First()!;

        var result = await _profileService.ChangeEmailAsync(userId, token);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }
}