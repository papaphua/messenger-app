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

        if (!result.Succeeded) return RedirectToAction("Index", "Home");

        var directPreviews = result.Data!;

        return View(directPreviews);
    }

    public async Task<IActionResult> Chat(string directId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        Result<DirectDto> result;

        if (TempData.TryGetValue("DirectId", out var value))
            result = await _directService.GetDirectAsync(userId, value!.ToString()!);
        else
            result = await _directService.GetDirectAsync(userId, directId);

        if (!result.Succeeded) return RedirectToAction("Index", "Direct");

        var direct = result.Data!;

        var profile = (await _profileService.GetProfileAsync(userId)).Data!;

        ViewBag.Username = profile.ProfileInfoDto.UserName;
        ViewBag.ProfilePictureBytes = Convert.ToBase64String(profile.ProfilePictureBytes!);

        return View(direct);
    }

    public async Task<IActionResult> CreateDirect(string conversatorId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.CreateDirectAsync(userId, conversatorId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        if (!result.Succeeded) return RedirectToAction("Index", "Direct");

        var direct = result.Data!;

        return View("Chat", direct);
    }

    public async Task<IActionResult> RemoveDirect(string directId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.RemoveDirectAsync(userId, directId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Direct");
    }

    public async Task<IActionResult> CreateDirectMessage(string directId, CreateMessageDto createMessageDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var attachedFiles = Request.Form.Files;

        var attachments = new List<byte[]>();

        if (attachedFiles.Any())
            foreach (var attachment in attachedFiles)
            {
                using var memoryStream = new MemoryStream();
                await attachment.CopyToAsync(memoryStream);
                attachments.Add(memoryStream.ToArray());
            }

        createMessageDto.Attachments = attachments;

        await _directService.CreateDirectMessageAsync(userId, directId, createMessageDto);

        var direct = (await _directService.GetDirectAsync(userId, directId)).Data!;

        TempData["DirectId"] = direct.Id;

        return RedirectToAction("Chat");
    }

    public async Task AddReaction(string messageId, Reaction reaction)
    {
        var userId = Parser.ParseUserId(HttpContext);

        await _directService.CreateDirectReactionAsync(userId, messageId, reaction);
    }
}