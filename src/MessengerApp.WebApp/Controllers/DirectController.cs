using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Direct;
using MessengerApp.Application.Services.DirectService;
using MessengerApp.Application.Services.ProfileService;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Enumerations;
using MessengerApp.Domain.Primitives;
using MessengerApp.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class DirectController : Controller
{
    private readonly IDirectService _directService;
    private readonly IProfileService _profileService;

    public DirectController(IDirectService directService, IProfileService profileService)
    {
        _directService = directService;
        _profileService = profileService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.GetDirectPreviewsAsync(userId);

        if (result.Succeeded) return View(result.Data);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Home");
    }

    [Route("Direct/Chat/{directId}")]
    public async Task<IActionResult> Chat(string directId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.GetDirectAsync(userId, directId);

        if (result.Succeeded)
        {
            var profile = (await _profileService.GetProfileAsync(userId)).Data!;

            ViewBag.Username = profile.ProfileInfoDto.UserName;
            ViewBag.ProfilePictureBytes = Convert.ToBase64String(profile.ProfilePictureBytes!);

            return View(result.Data);
        }

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> CreateDirect(string conversatorId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.CreateDirectAsync(userId, conversatorId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        if (!result.Succeeded) return RedirectToAction("Index");

        return View("Chat", result.Data);
    }

    public async Task<IActionResult> RemoveDirect(string directId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.RemoveDirectAsync(userId, directId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> CreateDirectMessage(string directId, CreateMessageDto createMessageDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var attachmentBytes = await Parser.GetAttachmentsAsync(Request.Form.Files);
        createMessageDto.Attachments = attachmentBytes;

        var result = await _directService.CreateDirectMessageAsync(userId, directId, createMessageDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Chat", new { directId });
    }

    public async Task<IActionResult> AddReaction(string messageId, Reaction reaction)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.CreateDirectReactionAsync(userId, messageId, reaction);

        if (result.Succeeded)
        {
            var directId = result.Data;
            return RedirectToAction("Chat", new { directId });
        }
        
        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }
}