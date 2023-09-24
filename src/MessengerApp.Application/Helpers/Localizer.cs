using System.Globalization;
using System.Net;
using System.Resources;
using MessengerApp.Application.Resources;

namespace MessengerApp.Application.Helpers;

public static class Localizer
{
    public static string GetLocalizedResult(Domain.Enumerations.Results key, string? parameter = null)
    {
        var culture = CultureInfo.CurrentCulture;

        var resourceManager = new ResourceManager("MessengerApp.Application.Resources.Results", typeof(Results).Assembly);
        var localizedString = resourceManager.GetString(key.ToString(), culture);
        
        if (!string.IsNullOrEmpty(parameter))
        {
            localizedString = string.Format(localizedString!, parameter);
        }
        
        return WebUtility.HtmlDecode(localizedString)!;
    }
}