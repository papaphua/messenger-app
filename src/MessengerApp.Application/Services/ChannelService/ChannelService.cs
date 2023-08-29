﻿using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Services.ChannelService;

public sealed class ChannelService : IChannelService
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public ChannelService(IDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<ChannelDto>> GetChannelAsync(string userId, string channelId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channel = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .FirstOrDefaultAsync(channel => channel.Id == channelId &&
                                            channel.Members.Any(member => member.Id == userId));

        if (channel == null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var channelDto = _mapper.Map<ChannelDto>(channel);

        return new Result<ChannelDto>
        {
            Data = channelDto
        };
    }

    public async Task<Result<IEnumerable<ChannelPreviewDto>>> GetChannelPreviewsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<IEnumerable<ChannelPreviewDto>>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channels = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .Where(channel => channel.Members.Any(member => member.Id == user.Id))
            .ToListAsync();

        if (channels.Count == 0)
            return new Result<IEnumerable<ChannelPreviewDto>>
            {
                Message = Results.ChatsEmpty
            };

        var channelPreviewDtos = channels
            .Select(channel => _mapper.Map<ChannelPreviewDto>(channel))
            .ToList();

        return new Result<IEnumerable<ChannelPreviewDto>>
        {
            Data = channelPreviewDtos
        };
    }

    public async Task<Result<ChannelDto>> CreateChannelAsync(string userId, ChannelInfoDto channelInfoDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channel = new Channel();
        _mapper.Map(channelInfoDto, channel);

        var channelMember = ChannelMember.AddMemberToChannel(channel.Id, user.Id);
        channelMember.IsAdmin = true;
        channelMember.IsOwner = true;

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.AddAsync(channel);
            await _unitOfWork.SaveChangesAsync();

            await _dbContext.AddAsync(channelMember);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.ChatCreateError
            };
        }

        await transaction.CommitAsync();

        return await GetChannelAsync(user.Id, channel.Id);
    }

    public async Task<Result> LeaveChannelAsync(string userId, string channelId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channel = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .FirstOrDefaultAsync(channel => channel.Id == channelId);

        if (channel == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var channelMember = await _dbContext.Set<ChannelMember>()
            .FirstOrDefaultAsync(member => member.ChannelId == channel.Id);

        if (channelMember == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        try
        {
            if (channelMember is { IsOwner: true, IsAdmin: true })
            {
                var otherOwners = await _dbContext.Set<ChannelMember>()
                    .Where(member => member.ChannelId == channel.Id &&
                                     member.MembersId != channelMember.MembersId &&
                                     member.IsOwner)
                    .ToListAsync();

                var otherAdmins = await _dbContext.Set<ChannelMember>()
                    .Where(member => member.ChannelId == channel.Id &&
                                     member.MembersId != channelMember.MembersId &&
                                     !member.IsOwner && member.IsAdmin)
                    .ToListAsync();

                var otherMembers = await _dbContext.Set<ChannelMember>()
                    .Where(member => member.ChannelId == channel.Id &&
                                     member.MembersId != channelMember.MembersId &&
                                     !member.IsOwner && !member.IsAdmin)
                    .ToListAsync();

                if (otherOwners.Count >= 1)
                {
                    _dbContext.Remove(channelMember);
                }
                else if (otherAdmins.Count >= 1)
                {
                    var admin = otherAdmins.First();
                    admin.IsOwner = true;

                    _dbContext.Remove(channelMember);
                }
                else if (otherMembers.Count >= 1)
                {
                    var member = otherMembers.First();
                    member.IsOwner = true;
                    member.IsAdmin = true;

                    _dbContext.Remove(channelMember);
                }
                else
                {
                    _dbContext.Remove(channel);
                }
            }
            else
            {
                _dbContext.Remove(channelMember);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatLeaveError
            };
        }

        return new Result();
    }
}