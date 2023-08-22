using AutoMapper;
using MessengerApp.Application.Dtos.Profile;
using MessengerApp.Application.Dtos.User;
using MessengerApp.Domain.Entities;

namespace MessengerApp.Application;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, ProfileDto>()
            .ForMember(dest => dest.ProfileEmailDto,
                opt => opt.MapFrom(src => new ProfileEmailDto
                {
                    Email = src.Email!,
                    IsConfirmed = src.EmailConfirmed
                }))
            .ForMember(dest => dest.ProfileInfoDto,
                opt => opt.MapFrom(src => new ProfileInfoDto
                {
                    UserName = src.UserName!,
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    Biography = src.Biography
                }))
            .ForMember(dest => dest.ProfilePictureDto,
                opt => opt.MapFrom(src => new ProfilePictureDto
                {
                    ProfilePicture = src.ProfilePicture
                }));

        CreateMap<ProfileInfoDto, User>();
        CreateMap<User, UserPreviewDto>();
    }
}