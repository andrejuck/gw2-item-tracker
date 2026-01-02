using System.Threading.Channels;
using Gw2ItemTracker.App.Adapters;
using Gw2ItemTracker.App.Application;
using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra;
using Libs.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gw2ItemTracker.App.Controllers;

[ApiController]
[Route("api/synchronize")]
public class SyncronizeController : Controller
{
    private readonly ISynchronizeApplication _application;

    public SyncronizeController(ISynchronizeApplication application)
    {
        _application = application;
    }

    // GET
    [HttpGet("items")]
    public async Task<IActionResult> SynchronizeAllItemsAsync([FromQuery] bool synchronizeAll = false)

    {
        var result = await _application.SynchronizeAllItemsAsync(synchronizeAll);
        return Accepted(new { Message = "Items synchronization requested successfully"});
    }
    
    [HttpGet("recipes")]
    public async Task<IActionResult> SynchronizeAllRecipesAsync([FromQuery] bool synchronizeAll = false)
    {
        var result = await _application.SynchronizeAllRecipesAsync(synchronizeAll);
        return Accepted(new { Message = "Recipes synchronization requested successfully"});
    }
    
    [HttpGet("material-categories")]
    public async Task<IActionResult> SynchronizeMaterialCategoriesAsync()
    {
        var result = await _application.SynchronizeMaterialCategoriesAsync();
        return Ok(new { Message = "Material Categories synchronization completed successfully"});
    }
}