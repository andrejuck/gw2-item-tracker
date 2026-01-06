using System.Threading.Channels;
using Gw2ItemTracker.App.Adapters;
using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra;
using Gw2ItemTracker.Infra.Repositories;
using Libs.Api.Models;

namespace Gw2ItemTracker.App.Application;

public class SynchronizeApplication : ISynchronizeApplication
{
    private readonly ISynchronizeAdapter _synchronizeAdapter;
    private readonly Gw2HttpClient _gw2HttpClient;
    private readonly Channel<ProcessingResource<ItemDto>> _itemDtoChannel;
    private readonly Channel<ProcessingResource<RecipeDto>> _recipeDtoChannel;
    private readonly IItemRepository _itemRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly ILogger<SynchronizeApplication> _logger;

    public SynchronizeApplication(ISynchronizeAdapter synchronizeAdapter,
        Channel<ProcessingResource<ItemDto>> itemDtoChannel, 
        Channel<ProcessingResource<RecipeDto>> recipeDtoChannel,
        IItemRepository itemRepository,
        IRecipeRepository recipeRepository,
        IMaterialRepository materialRepository,
        ILogger<SynchronizeApplication> logger)
    {
        _synchronizeAdapter = synchronizeAdapter;
        _gw2HttpClient = new Gw2HttpClient();
        _itemDtoChannel = itemDtoChannel;
        _recipeDtoChannel = recipeDtoChannel;
        _itemRepository = itemRepository;
        _recipeRepository = recipeRepository;
        _materialRepository =  materialRepository;
        _logger = logger;
    }

    public async Task<bool> SynchronizeAllItemsAsync(bool synchronizeAll)
    {
        var pagedRequest = new PagedRequest()
        {
            CurrentPage = 0,
            PageSize = 50
        };

        var flowControl = true;
        if (!synchronizeAll)
        {
            var lastPageProcessed = await _itemRepository.GetLastPageProcessedAsync();
            pagedRequest.CurrentPage = lastPageProcessed;
            _logger.LogInformation($"Synchronizing from last page {lastPageProcessed} persisted");
        }

        _ = Task.Run(async () =>
        {
            do
            {
                _logger.LogInformation($"Fetching items on page {pagedRequest.CurrentPage} of size {pagedRequest.PageSize}");
                var result = await _gw2HttpClient.GetAsync<List<ItemDto>>(pagedRequest, "v2/items");
                if (result is null || result.Count == 0)
                {
                    flowControl = false;
                    continue;
                }

                var processingQueueItems =
                    _synchronizeAdapter.ConvertToProcessingResource(result, pagedRequest.CurrentPage);
                pagedRequest.CurrentPage++;

                foreach (var queueItem in processingQueueItems)
                {
                    if (string.IsNullOrEmpty(queueItem.Resource.name)) continue;
                    _logger.LogInformation($"Adding item {queueItem.Id} to processing queue");
                    _itemDtoChannel.Writer.TryWrite(queueItem);
                }
            } while (flowControl);
        });

        return true;
    }

    public async Task<bool> SynchronizeAllRecipesAsync(bool synchronizeAll)
    {
        var pagedRequest = new PagedRequest()
        {
            CurrentPage = 0,
            PageSize = 50
        };

        var flowControl = true;
        if (!synchronizeAll)
        {
            var lastPageProcessed = await _recipeRepository.GetLastPageProcessedAsync();
            pagedRequest.CurrentPage = lastPageProcessed;
            _logger.LogInformation($"Synchronizing from last page {lastPageProcessed} persisted");
        }

        _ = Task.Run(async () =>
        {
            do
            {
                _logger.LogInformation($"Fetching recipes on page {pagedRequest.CurrentPage} of size {pagedRequest.PageSize}");
                var result = await _gw2HttpClient.GetAsync<List<RecipeDto>>(pagedRequest, "v2/recipes");
                if (result is null || result.Count == 0)
                {
                    flowControl = false;
                    continue;
                }

                var processingQueueItems =
                    _synchronizeAdapter.ConvertToProcessingResource(result, pagedRequest.CurrentPage);
                pagedRequest.CurrentPage++;

                foreach (var queueItem in processingQueueItems)
                {
                    _logger.LogInformation($"Adding recipe {queueItem.Id} to processing queue");
                    _recipeDtoChannel.Writer.TryWrite(queueItem);
                }
            } while (flowControl);
        });

        return true;
    }

    public async Task<bool> SynchronizeMaterialCategoriesAsync()
    {
        var pagedRequest = new PagedRequest()
        {
            CurrentPage = 0,
            PageSize = 50
        };
        _logger.LogInformation($"Fetching materials categories");
        var result = await _gw2HttpClient.GetAsync<List<MaterialCategoryDto>>(pagedRequest, "v2/materials");
        if (result is null) return false;
        
        var processingItem = _synchronizeAdapter.ConvertToProcessingResource(result, pagedRequest.CurrentPage);
        foreach (var item in processingItem)
        {
            item.StartProcessing();
            _logger.LogInformation($"Adding material category {item.Id} to processing queue");
            var entity = new MaterialCategory(item.Resource.id, item.Resource.name);
            await _materialRepository.AddOrUpdateAsync(entity);
        }

        return true;
    }
}