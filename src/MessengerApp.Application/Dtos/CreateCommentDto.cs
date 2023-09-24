using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class CreateCommentDto
{
    [Required(ErrorMessage = "CommentRequired")] public string Content { get; set; } = null!;
}