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
    
    public async Task<IActionResult> Chat(Guid conversatorId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _directService.CreateDirectAsync(userId, conversatorId.ToString());
        
        if (!result.Succeeded)
        {
            TempData[Notifications.Message] = result.Message;
            TempData[Notifications.Succeeded] = result.Succeeded;

            return RedirectToAction("Index", "Home");
        }

        // TODO open chat page after creation
        // or just open if already created
        // add method to open chat without try to create
        return View();
    }
}