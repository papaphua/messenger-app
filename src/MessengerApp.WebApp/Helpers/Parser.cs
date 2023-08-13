using System.Security.Claims;

namespace MessengerApp.WebApp.Helpers;

public static class Parser
{
    public static string? ParseUserId(HttpContext context)
    {
        return context.User.FindFirstValue("sub");
    }
}