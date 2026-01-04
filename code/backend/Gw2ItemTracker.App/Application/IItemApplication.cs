using Gw2ItemTracker.App.Views;
using Libs.Api.Models;

namespace Gw2ItemTracker.App.Application;

public interface IItemApplication
{
    Task<ItemView?> GetItemByIdAsync(int itemId);
    Task<PagedResponse<ItemView>> GetPagedItemsAsync(PagedRequest pagedRequest, string? searchString);
}