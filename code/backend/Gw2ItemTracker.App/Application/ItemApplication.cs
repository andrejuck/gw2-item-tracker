using Gw2ItemTracker.App.Adapters;
using Gw2ItemTracker.App.Views;
using Gw2ItemTracker.Domain.DataContracts;
using Libs.Api.Models;

namespace Gw2ItemTracker.App.Application;

public class ItemApplication : IItemApplication
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemAdapter _adapter;
    private readonly ILogger<ItemApplication> _logger;

    public ItemApplication(IItemRepository itemRepository, 
        IItemAdapter adapter, 
        ILogger<ItemApplication> logger)
    {
        _itemRepository = itemRepository;
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<ItemView?> GetItemByIdAsync(int itemId)
    {
        var entity = await _itemRepository.FindByIdAsync(itemId);

        if (entity is null)
        {
            _logger.LogError($"Item with id {itemId} not found");
            return null;
        }
        
        _logger.LogInformation($"Item with id {itemId} was found");
        return _adapter.ConvertToView(entity);
    }

    public async Task<PagedResponse<ItemView>> GetPagedItemsAsync(PagedRequest pagedRequest, string? searchString)
    {
        return await _itemRepository.GetAllPagedAsync<ItemView>(pagedRequest, searchString);
    }
}