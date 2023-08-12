using MessengerApp.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        
        var user = _db.Users.FirstOrDefault(user => user.Id.ToString() == userId);

        return View(user);
    }

    public IActionResult LogOut()
    {
        return SignOut("Cookies", "oidc");
    }
}