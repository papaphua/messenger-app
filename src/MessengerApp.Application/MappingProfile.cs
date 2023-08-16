using AutoMapper;
using MessengerApp.Application.Dtos.User;
using MessengerApp.Domain.Entities;

namespace MessengerApp.Application;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.UserEmailDto,
                opt => opt.MapFrom(src => new UserEmailDto
                {
                    Email = src.Email,
                    IsConfirmed = src.EmailConfirmed
                }))
            .ForMember(dest => dest.UserInfoDto,
                opt => opt.MapFrom(src => new UserInfoDto
                {
                    UserName = src.UserName,
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    Biography = src.Biography
                }))
            .ForMember(dest => dest.UserProfilePictureDto,
                opt => opt.MapFrom(src => new UserProfilePictureDto
                {
                    ProfilePictureBytes = src.ProfilePicture
                }));

        CreateMap<UserInfoDto, User>();
    }
}