using Gw2ItemTracker.App.Application;
using Microsoft.AspNetCore.Mvc;

namespace Gw2ItemTracker.App.Controllers;

[ApiController]
[Route("synchronize")]
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