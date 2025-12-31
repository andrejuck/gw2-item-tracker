using System.Threading.Channels;
using Gw2ItemTracker.App.Adapters;
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
    private readonly ISynchronizeAdapter _synchronizeAdapter;
    private readonly Gw2HttpClient _gw2HttpClient;
    private readonly Channel<ProcessingResource<ItemDto>> _itemDtoChannel;
    private readonly IItemRepository _itemRepository;

    public SyncronizeController(
        ISynchronizeAdapter synchronizeAdapter,
        Channel<ProcessingResource<ItemDto>> itemDtoChannel,
        IItemRepository itemRepository
    )
    {
        _synchronizeAdapter = synchronizeAdapter;
        _gw2HttpClient = new Gw2HttpClient();
        _itemDtoChannel = itemDtoChannel;
        _itemRepository = itemRepository;
    }

    // GET
    [HttpGet("items")]
    public async Task<IActionResult> SynchronizeAllItemsAsync([FromQuery] bool synchronizeAll = false)

    {
        var pagedRequest = new PagedRequest()
        {
            CurrentPage = 0,
            PageSize = 50
        };

        var items = new List<ItemDto>();
        var flowControl = true;
        if (!synchronizeAll)
        {
            var lastPageProcessed = await _itemRepository.GetLastPageProcessedAsync();
            pagedRequest.CurrentPage = lastPageProcessed;
        }
        
        do
        {
            var result = await _gw2HttpClient.GetAsync<List<ItemDto>>(pagedRequest, "v2/items");
            if (result is null || result.Count == 0)
            {
                flowControl = false;
                continue;
            }

            var processingQueueItems =
                _synchronizeAdapter.ConvertToProcessingResource(result, pagedRequest.CurrentPage);
            items.AddRange(result);
            pagedRequest.CurrentPage++;

            foreach (var queueItem in processingQueueItems)
            {
                _itemDtoChannel.Writer.TryWrite(queueItem);
            };
        } while (flowControl);

        return Ok();
    }
}