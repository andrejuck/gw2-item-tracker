using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra;
using Libs.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gw2ItemTracker.App.Controllers;

[ApiController]
[Route("api/synchronize")]
public class SyncronizeController : Controller
{
    private readonly Gw2HttpClient _gw2HttpClient;

    public SyncronizeController()
    {
        _gw2HttpClient = new Gw2HttpClient();
    }

    // GET
    [HttpGet("items")]
    public async Task<IActionResult> SynchronizeItemsAsync([FromQuery] PagedRequest request)
    {
        var result = await _gw2HttpClient.GetAsync<IEnumerable<long>>(request, "items");
        //Log Warning
        if(result is null)  return NoContent(); 
        
        //Adapt to processing queue
        var processingQueueItems = ConvertToProcessingResource(result); 

        return Ok(result);
    }

    public IEnumerable<ProcessingResource> ConvertToProcessingResource(IEnumerable<long> items) =>
        items.Select(ConvertToProcessingResource);

    private ProcessingResource ConvertToProcessingResource(long id)
        => new ProcessingResource(id, "items", ProcessingStatus.Queued);
}