﻿using MessengerApp.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Domain.Entities;

public sealed class User : IdentityUser<Guid>, IEntity
{
    public new string Email { get; set; }
    
    public new string UserName { get; set; }
    
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Biography { get; set; }

    public string? RequestedEmail { get; set; }

    public bool IsExternal { get; set; } = false;

    public ICollection<Chat> Chats { get; set; } = null!;

    Guid IEntity.Id
    {
        get => Id;
        set => Id = value;
    }
}