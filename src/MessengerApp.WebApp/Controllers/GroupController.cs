using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Services.GroupService;
using MessengerApp.Domain.Constants;
using MessengerApp.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApp.WebApp.Controllers;

public sealed class GroupController : Controller
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    public async Task<IActionResult> Index()
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _groupService.GetGroupPreviewsAsync(userId);

        TempData[Notifications.Message] = result.Message;
        TempData[Notifications.Succeeded] = result.Succeeded;

        var groupPreviews = result.Data;

        return View(groupPreviews);
    }

    public async Task<IActionResult> Chat(string groupId)
    {
        var userId = Parser.ParseUserId(HttpContext);

        var result = await _groupService.GetGroupAsync(userId, groupId);
        
        if (!result.Succeeded)
        {
            TempData[Notifications.Message] = result.Message;
            TempData[Notifications.Succeeded] = result.Succeeded;

            return RedirectToAction("Index", "Home");
        }

        var group = result.Data!;
        
        return View(group);
    }

    public IActionResult NewGroup()
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
        
        if (!result.Succeeded)
        {
            TempData[Notifications.Message] = result.Message;
            TempData[Notifications.Succeeded] = result.Succeeded;

            return RedirectToAction("Index", "Group");
        }
        
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
}