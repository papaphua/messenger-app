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

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        var directPreviews = result.Data;

        return View(directPreviews);
    }

    public async Task<IActionResult> Chat(string directId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.GetDirectAsync(userId, directId);
        
        if (!result.Succeeded)
        {
            TempData[Notifications.Message] = result.Message;
            TempData[Notifications.Succeeded] = result.Succeeded;

            return RedirectToAction("Index", "Home");
        }

        var direct = result.Data!;
        
        return View("Chat", direct);
    }

    public async Task<IActionResult> StartChat(string conversatorId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.CreateDirectAsync(userId, conversatorId);

        if (!result.Succeeded)
        {
            TempData[Notifications.Message] = result.Message;
            TempData[Notifications.Succeeded] = result.Succeeded;

            return RedirectToAction("Index", "Home");
        }

        var directId = result.Data!;

        var getDirectResult = await _directService.GetDirectAsync(userId, directId);
        var direct = getDirectResult.Data!;

        return View("Chat", direct);
    }

    public async Task<IActionResult> RemoveChat(string directId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.RemoveDirectAsync(userId, directId);

        var getDirectPreviewsResult = await _directService.GetDirectPreviewsAsync(userId);
        var directs = getDirectPreviewsResult.Data!;
        
        TempData[Notifications.Message] = result.Message ?? getDirectPreviewsResult.Message;
        TempData[Notifications.Succeeded] =
            result.Message == null ? getDirectPreviewsResult.Succeeded : result.Succeeded;
        
        return View("Index", directs);
    }
}