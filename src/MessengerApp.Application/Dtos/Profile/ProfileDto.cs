﻿namespace MessengerApp.Application.Dtos.Profile;

public sealed class ProfileDto
{
    public ProfileInfoDto ProfileInfoDto { get; set; } = null!;

    public ProfileEmailDto ProfileEmailDto { get; set; } = null!;

    public ProfilePictureDto ProfilePictureDto { get; set; } = null!;
}