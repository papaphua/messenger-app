using AutoMapper;
using MessengerApp.Application.Dtos.Group;
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
                }));

        CreateMap<ProfileInfoDto, User>();
        CreateMap<User, ProfileInfoDto>();
        CreateMap<User, UserPreviewDto>();
        
        CreateMap<Group, GroupDto>()
            .ForMember(dest => dest.GroupInfoDto,
                opt => opt.MapFrom(src => new GroupInfoDto
                {
                    Title = src.Title,
                    Description = src.Description,
                    ChatPictureBytes = src.ChatPictureBytes
                }));

        CreateMap<Group, GroupPreviewDto>();
        CreateMap<GroupInfoDto, Group>();
        CreateMap<Group, GroupInfoDto>();
    }
}