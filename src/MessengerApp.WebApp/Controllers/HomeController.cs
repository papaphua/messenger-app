using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        var currentCulture = CultureInfo.CurrentCulture.Name;

        ViewBag.CurrentCulture = currentCulture;

        return View();
    }

    public IActionResult LogOut()
    {
        return SignOut("Cookies", "oidc");
    }

    public IActionResult ChangeCulture(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return RedirectToAction("Index");
    }
}