using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Services.GroupService;
using MessengerApp.Application.Services.ProfileService;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Enumerations;
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

        if (result.Succeeded) return View(result.Data);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index", "Home");
    }

    [Route("Group/Chat/{groupId}")]
    public async Task<IActionResult> Chat(string groupId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _groupService.GetGroupAsync(userId, groupId);

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

    public async Task<IActionResult> CreateGroup(GroupInfoDto groupInfoDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var chatPictureBytes = await Parser.GetAttachmentAsync(Request.Form.Files);
        groupInfoDto.ChatPictureBytes = chatPictureBytes;

        var result = await _groupService.CreateGroupAsync(userId, groupInfoDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        if (!result.Succeeded) return RedirectToAction("Index");

        return View("Chat", result.Data);
    }

    public async Task<IActionResult> JoinGroup(string groupId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _groupService.JoinGroupAsync(userId, groupId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        if (!result.Succeeded) return RedirectToAction("Index");

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

        var attachmentBytes = await Parser.GetAttachmentsAsync(Request.Form.Files);
        createMessageDto.Attachments = attachmentBytes;

        var result = await _groupService.CreateGroupMessageAsync(userId, groupId, createMessageDto);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Chat", new { groupId });
    }

    public async Task<IActionResult> AddReaction(string messageId, Reaction reaction)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _groupService.CreateGroupReactionAsync(userId, messageId, reaction);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        var groupId = result.Data;
        return RedirectToAction("Chat", new { groupId });
    }

    [Route("Group/Chat/Options/{groupId}")]
    public async Task<IActionResult> Edit(string groupId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _groupService.GetGroupOptionsAsync(userId, groupId);

        if (result.Succeeded)
        {
            TempData["GroupId"] = groupId;
            return View(result.Data);
        }

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateGroupOptions(GroupOptionsDto groupOptionsDto)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var groupId = TempData["GroupId"]!.ToString()!;
        
        var result = await _groupService.UpdateGroupOptionsAsync(userId, groupId, groupOptionsDto);
        
        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;
        
        return RedirectToAction("Edit", new { groupId });
    }
}