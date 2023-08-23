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
            var user = userResult.Data;
            
            return View("Index", user);
        }

        var profilePicture = Request.Form.Files[0];

        // TODO send stream to server
        using var memoryStream = new MemoryStream();

        await profilePicture.CopyToAsync(memoryStream);
        var profilePictureBytes = memoryStream.ToArray();
        var uploadResult = await _profileService.UploadProfilePictureAsync(userId, profilePictureBytes);

        TempData[Notifications.Message] = uploadResult.Message;
        TempData[Notifications.Succeeded] = uploadResult.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateProfile(ProfileInfoDto profileInfoDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var userResult = await _profileService.GetProfileAsync(userId);
            var user = userResult.Data;
            
            return View("Index", user);
        }

        var result = await _profileService.UpdateUserInfoAsync(userId, profileInfoDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        if (!ModelState.IsValid)
        {
            var userResult = await _profileService.GetProfileAsync(userId);
            var user = userResult.Data;
            
            return View("Index", user);
        }

        var result = await _profileService.ChangePasswordAsync(userId, changePasswordDto);

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

    public async Task<IActionResult> RequestEmailChange(ProfileEmailDto profileEmailDto)
    {
        var userId = Parser.ParseUserId(HttpContext)!;

        var result = await _profileService.RequestEmailChangeAsync(userId, profileEmailDto);

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