using MessengerApp.Application.Dtos;
using MessengerApp.Application.Services.DirectService;
using MessengerApp.Domain.Constants;
using MessengerApp.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class DirectController : Controller
{
    private readonly IDirectService _directService;

    public DirectController(IDirectService directService)
    {
        _directService = directService;
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

        var result = await _directService.GetDirectAsync(userId, directId);

        if (!result.Succeeded) return RedirectToAction("Index", "Direct");

        var direct = result.Data!;

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
        
        await _directService.CreateDirectMessageAsync(userId, directId, createMessageDto);

        var direct = (await _directService.GetDirectAsync(userId, directId)).Data;

        return View("Chat", direct);
    }
}