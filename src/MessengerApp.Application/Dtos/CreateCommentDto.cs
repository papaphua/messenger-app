using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class CreateCommentDto
{
    [Required]
    public string Content { get; set; } = null!;
}