using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Application.Services.ChannelService;
using MessengerApp.Domain.Constants;
using MessengerApp.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class ChannelController : Controller
{
    private readonly IChannelService _channelService;

    public ChannelController(IChannelService channelService)
    {
        _channelService = channelService;
    }
    
    public async Task<IActionResult> Index()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.GetChannelPreviewsAsync(userId);

        if (!result.Succeeded) return RedirectToAction("Index", "Channel");

        var channelPreviews = result.Data;

        return View(channelPreviews);
    }

    public async Task<IActionResult> Chat(string channelId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _channelService.GetChannelAsync(userId, channelId);

        if (!result.Succeeded) return RedirectToAction("Index", "Channel");

        var channel = result.Data!;

        return View(channel);
    }

    public IActionResult New()
    {
        return View();
    }

    public async Task<IActionResult> CreateChannel(ChannelInfoDto channelInfoDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var chatPicture = Request.Form.Files[0];

        using var memoryStream = new MemoryStream();

        await chatPicture.CopyToAsync(memoryStream);
        var chatPictureBytes = memoryStream.ToArray();

        channelInfoDto.ChatPictureBytes = chatPictureBytes;

        var result = await _channelService.CreateChannelAsync(userId, channelInfoDto);

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
}