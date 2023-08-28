﻿using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class DirectMember : IEntity
{
    public string DirectId { get; set; } = null!;

    public string MembersId { get; set; } = null!;
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public static DirectMember AddMemberToDirect(string directId, string userId)
    {
        return new DirectMember
        {
            DirectId = directId,
            MembersId = userId
        };
    }
}