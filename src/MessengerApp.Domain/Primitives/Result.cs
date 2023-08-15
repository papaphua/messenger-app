using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Domain.Primitives;

public class Result
{
    public bool Succeeded { get; set; } = true;
    public string? Message { get; set; }

    public static string? IdentityResultsToMessage(IdentityResult result)
    {
        var message = string.Join(Environment.NewLine, result.Errors.Select(error => error.Description));

        return string.IsNullOrWhiteSpace(message) ? null : message;
    }
}

public sealed class Result<TData> : Result
{
    public TData? Data { get; set; }
}