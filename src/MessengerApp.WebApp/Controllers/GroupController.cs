using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Services.GroupService;
using MessengerApp.Application.Services.ProfileService;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Primitives;
using MessengerApp.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class GroupController : Controller
{
    private readonly IGroupService _groupService;
    private readonly IProfileService _profileService;

    public GroupController(IGroupService groupService, IProfileService profileService)
    {
        _groupService = groupService;
        _profileService = profileService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _groupService.GetGroupPreviewsAsync(userId);

        if (!result.Succeeded) return RedirectToAction("Index", "Group");

        var groupPreviews = result.Data;

        return View(groupPreviews);
    }

    public async Task<IActionResult> Chat(string groupId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        Result<GroupDto> result; 
        
        if (TempData.TryGetValue("GroupId", out object? value))
        {
            result = await _groupService.GetGroupAsync(userId, value!.ToString()!);
        }
        else
        {
            result = await _groupService.GetGroupAsync(userId, groupId);
        }

        if (!result.Succeeded) return RedirectToAction("Index", "Group");

        var group = result.Data!;

        var profile = (await _profileService.GetProfileAsync(userId)).Data!;

        ViewBag.Username = profile.ProfileInfoDto.UserName;
        ViewBag.ProfilePictureBytes = Convert.ToBase64String(profile.ProfilePictureBytes);
        
        return View(group);
    }

    public IActionResult New()
    {
        return View();
    }

    public async Task<IActionResult> CreateGroup(GroupInfoDto groupInfoDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var chatPicture = Request.Form.Files[0];

        using var memoryStream = new MemoryStream();

        await chatPicture.CopyToAsync(memoryStream);
        var chatPictureBytes = memoryStream.ToArray();

        groupInfoDto.ChatPictureBytes = chatPictureBytes;

        var result = await _groupService.CreateGroupAsync(userId, groupInfoDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        if (!result.Succeeded) return RedirectToAction("Index", "Group");

        return View("Chat", result.Data);
    }

    public async Task<IActionResult> LeaveGroup(string groupId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _groupService.LeaveGroupAsync(userId, groupId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> CreateGroupMessage(string groupId, CreateMessageDto createMessageDto)
    {
        var userId = Parser.ParseUserId(HttpContext);
        
        await _groupService.CreateGroupMessageAsync(userId, groupId, createMessageDto);

        var group = (await _groupService.GetGroupAsync(userId, groupId)).Data!;

        TempData["GroupId"] = group.Id;

        return RedirectToAction("Chat");
    }
}