// ReSharper disable InconsistentNaming

namespace MessengerApp.Infrastructure.Options;

public sealed class EmailOptions
{
    public required string SenderEmail { get; set; }

    public required string SenderName { get; set; }
}