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

        if (!result.Succeeded)
        {
            TempData[Notifications.Message] = result.Message;
            TempData[Notifications.Succeeded] = result.Succeeded;

            return RedirectToAction("Index", "Home");
        }

        var user = result.Data;

        ViewBag.ProfilePictureBytes = user.UserProfileDto.ProfilePicture;
        
        return View(user);
    }

    public async Task<IActionResult> UploadProfilePicture()
    {
        var userId = Parser.ParseUserId(HttpContext)!;
        
        if (Request.Form.Files.Count == 0)
        {
            ModelState.AddModelError("file", "Please select a valid image file.");
            
            var result = await _userService.GetUserAsync(userId);
            
            if (!result.Succeeded)
            {
                TempData[Notifications.Message] = result.Message;
                TempData[Notifications.Succeeded] = result.Succeeded;

                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.ProfilePictureBytes = result.Data!.UserProfileDto.ProfilePicture!;
            
            return View("Profile", result.Data);
        }
        
        var profilePicture = Request.Form.Files[0];
        
        using var memoryStream = new MemoryStream();
        
        await profilePicture.CopyToAsync(memoryStream);
        var pictureBytes = memoryStream.ToArray();
        var uploadResult = await _userService.UploadProfilePictureAsync(userId, pictureBytes);
        
        TempData[Notifications.Message] = uploadResult.Message;
        TempData[Notifications.Succeeded] = uploadResult.Succeeded;
        
        return RedirectToAction("Profile");
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

    public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
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