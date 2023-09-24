using MessengerApp.Application.Services.SearchService;
using MessengerApp.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class SearchController : Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var result = await _searchService.SearchChatsAsync(search);

        if (result.Succeeded) return View(result.Data);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Home");
    }
}