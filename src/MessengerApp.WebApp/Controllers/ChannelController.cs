using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Application.Services.ChannelService;
using MessengerApp.Application.Services.ProfileService;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Enumerations;
using MessengerApp.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class ChannelController : Controller
{
    private readonly IChannelService _channelService;
    private readonly IProfileService _profileService;

    public ChannelController(IChannelService channelService, IProfileService profileService)
    {
        _channelService = channelService;
        _profileService = profileService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.GetChannelPreviewsAsync(userId);

        if (result.Succeeded) return View(result.Data);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Home");
    }

    [Route("Channel/Chat/{channelId}")]
    public async Task<IActionResult> Chat(string channelId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.GetChannelAsync(userId, channelId);

        if (result.Succeeded)
        {
            var profile = (await _profileService.GetProfileAsync(userId)).Data!;

            ViewBag.Username = profile.ProfileInfoDto.UserName;
            ViewBag.ProfilePictureBytes = Convert.ToBase64String(profile.ProfilePictureBytes!);

            return View(result.Data);
        }

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public IActionResult New()
    {
        return View();
    }

    [Route("Channel/Comments/{messageId}")]
    public async Task<IActionResult> Comments(string messageId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.GetChannelMessageCommentsAsync(userId, messageId);

        if (result.Succeeded) return View(result.Data);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> CreateChannel(ChannelInfoDto channelInfoDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var chatPictureBytes = await Parser.GetAttachmentAsync(Request.Form.Files);
        channelInfoDto.ChatPictureBytes = chatPictureBytes;

        var result = await _channelService.CreateChannelAsync(userId, channelInfoDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        if (!result.Succeeded) return RedirectToAction("Index");

        return View("Chat", result.Data);
    }

    public async Task<IActionResult> JoinChannel(string channelId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.JoinChannelAsync(userId, channelId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        if (!result.Succeeded) return RedirectToAction("Index", "Channel");

        return View("Chat", result.Data);
    }

    public async Task<IActionResult> LeaveChannel(string channelId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.LeaveChannelAsync(userId, channelId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> CreateChannelMessage(string channelId, CreateMessageDto createMessageDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var attachmentBytes = await Parser.GetAttachmentsAsync(Request.Form.Files);
        createMessageDto.Attachments = attachmentBytes;

        var result = await _channelService.CreateChannelMessageAsync(userId, channelId, createMessageDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Chat", new { channelId });
    }

    public async Task<IActionResult> CreateComment(string messageId, CreateCommentDto createCommentDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.CreateChannelMessageCommentAsync(userId, messageId, createCommentDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Comments", new { messageId });
    }

    public async Task<IActionResult> AddReaction(string messageId, Reaction reaction)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.CreateChannelReactionAsync(userId, messageId, reaction);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        var channelId = result.Data;
        return RedirectToAction("Chat", new { channelId });
    }
}