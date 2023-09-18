﻿using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class Channel
    : Chat<Channel, ChannelMessage, ChannelAttachment, ChannelReaction>, IAdministratedChat
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }

    public bool IsPrivate { get; set; }
    public bool AllowReactions { get; set; }
    public bool AllowComments { get; set; }
}