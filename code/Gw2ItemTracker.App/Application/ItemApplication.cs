using Gw2ItemTracker.App.Adapters;
using Gw2ItemTracker.App.Views;
using Gw2ItemTracker.Domain.DataContracts;
using Libs.Api.Models;

namespace Gw2ItemTracker.App.Application;

public class ItemApplication : IItemApplication
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemAdapter _adapter;

    public ItemApplication(IItemRepository itemRepository, IItemAdapter adapter)
    {
        _itemRepository = itemRepository;
        _adapter = adapter;
    }

    public async Task<ItemView?> GetItemByIdAsync(int itemId)
    {
        var entity = await _itemRepository.FindByIdAsync(itemId);
        return entity is null ? null : _adapter.ConvertToView(entity);
    }

    public async Task<PagedResponse<ItemView>> GetPagedItemsAsync(PagedRequest pagedRequest, string? searchString)
    {
        return await _itemRepository.GetAllPagedAsync<ItemView>(pagedRequest, searchString);
    }
}