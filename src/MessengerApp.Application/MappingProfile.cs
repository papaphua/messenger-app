using AutoMapper;
using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Application.Dtos.Direct;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Dtos.Profile;
using MessengerApp.Application.Dtos.User;
using MessengerApp.Domain.Entities;

namespace MessengerApp.Application;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User service
        CreateMap<User, UserPreviewDto>();

        // Profile service
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

        // Direct service
        CreateMap<User, DirectDto>()
            .ForMember(dest => dest.ProfileInfoDto, opt => opt.MapFrom(src => new ProfileInfoDto
            {
                UserName = src.UserName!,
                FirstName = src.FirstName,
                LastName = src.LastName,
                Biography = src.Biography
            }))
            .ForMember(dest => dest.ProfilePictureBytes, opt => opt.MapFrom(src => src.ProfilePictureBytes))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<CreateMessageDto, DirectMessage>();

        // Group service
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

        // Channel service
        CreateMap<Channel, ChannelDto>()
            .ForMember(dest => dest.ChannelInfoDto,
                opt => opt.MapFrom(src => new ChannelInfoDto()
                {
                    Title = src.Title,
                    Description = src.Description,
                    ChatPictureBytes = src.ChatPictureBytes
                }));
        CreateMap<Channel, ChannelPreviewDto>();
        CreateMap<ChannelInfoDto, Channel>();
        CreateMap<Channel, ChannelInfoDto>();
    }
}