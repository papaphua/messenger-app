using System.Security.Claims;

namespace MessengerApp.WebApp.Helpers;

public static class Parser
{
    public static string ParseUserId(HttpContext context)
    {
        return context.User.FindFirstValue("sub")!;
    }

    public static async Task<byte[]> GetAttachmentAsync(IFormFileCollection formFiles)
    {
        if (!formFiles.Any()) return Array.Empty<byte>();
        
        using var memoryStream = new MemoryStream();
        await formFiles[0].CopyToAsync(memoryStream);
        
        return memoryStream.ToArray();
    }
    
    public static async Task<List<byte[]>> GetAttachmentsAsync(IFormFileCollection formFiles)
    {
        var attachments = new List<byte[]>();

        if (!formFiles.Any()) return attachments;
        
        foreach (var attachment in formFiles)
        {
            using var memoryStream = new MemoryStream();
            await attachment.CopyToAsync(memoryStream);
            attachments.Add(memoryStream.ToArray());
        }

        return attachments;
    }
}