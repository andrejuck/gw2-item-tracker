using Gw2ItemTracker.App.Application;
using Libs.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gw2ItemTracker.App.Controllers;

[ApiController]
[Route("item")]
public class ItemController : Controller
{
    private readonly IItemApplication _application;

    public ItemController(IItemApplication application)
    {
        _application = application;
    }

    [HttpGet("{itemId:int}")]
    public async Task<IActionResult> GetItemByIdAsync(int itemId)
    {
        var view = await _application.GetItemByIdAsync(itemId);
        
        if (view is null) return NoContent();
        
        return Ok(view);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPagedAsync([FromQuery] PagedRequest pagedRequest, [FromQuery] string? searchString)
    {
        var pagedResponse = await _application.GetPagedItemsAsync(pagedRequest, searchString);
        return Ok(pagedResponse);
    }
}