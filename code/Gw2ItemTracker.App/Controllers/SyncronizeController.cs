using Gw2ItemTracker.App.Applications;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra;
using Libs.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gw2ItemTracker.App.Controllers;

[ApiController]
[Route("api/synchronize")]
public class SyncronizeController : Controller
{
    private readonly ISynchronizeApplication _synchronizeApplication;
    private readonly Gw2HttpClient _gw2HttpClient;

    public SyncronizeController(ISynchronizeApplication synchronizeApplication)
    {
        _synchronizeApplication = synchronizeApplication;
        _gw2HttpClient = new Gw2HttpClient();
    }

    // GET
    [HttpGet("items")]
    public async Task<IActionResult> SynchronizeItemsAsync([FromQuery] PagedRequest request)
    {
        var result = await _gw2HttpClient.GetAsync<List<int>>(request, "v2/items");
        //Log Warning
        if(result is null)  return NoContent(); 
        
        //Adapt to processing queue
        var processingQueueItems = _synchronizeApplication.ConvertToProcessingResource(result); 

        return Ok(processingQueueItems);
    }
    
}