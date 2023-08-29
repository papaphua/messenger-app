using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Domain.Primitives;

public class Result
{
    public bool Succeeded { get; init; } = true;

    public string? Message { get; init; }

    public static string? IdentityResultToString(IdentityResult result)
    {
        var message = string.Join(Environment.NewLine, result.Errors.Select(error => error.Description));

        return string.IsNullOrWhiteSpace(message) ? null : message;
    }
}

public sealed class Result<TData> : Result
{
    public TData? Data { get; init; }
}