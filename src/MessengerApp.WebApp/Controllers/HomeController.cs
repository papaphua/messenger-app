using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult LogOut()
    {
        return SignOut("Cookies", "oidc");
    }
}