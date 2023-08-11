using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}