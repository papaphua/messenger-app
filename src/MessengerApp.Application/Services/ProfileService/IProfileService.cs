﻿using MessengerApp.Application.Dtos.Profile;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.ProfileService;

public interface IProfileService
{
    public Task<Result<ProfileDto>> GetProfileAsync(string? userId);

    public Task<Result> UploadProfilePictureAsync(string? userId, ProfilePictureDto profilePictureDto);

    public Task<Result> UpdateUserInfoAsync(string? userId, ProfileInfoDto infoDto);

    public Task<Result> ChangePasswordAsync(string? userId, ChangePasswordDto passwordDto);

    public Task<Result> RequestEmailConfirmationAsync(string? userId);

    public Task<Result> ConfirmEmailAsync(string? userId, string token);

    public Task<Result> RequestEmailChangeAsync(string? userId, ProfileEmailDto emailDto);

    public Task<Result> ChangeEmailAsync(string? userId, string token);
}