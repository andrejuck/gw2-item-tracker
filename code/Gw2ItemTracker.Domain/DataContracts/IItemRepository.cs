using Gw2ItemTracker.Domain.Models;
using Libs.Api.Models;

namespace Gw2ItemTracker.Domain.DataContracts;

public interface IItemRepository
{
    Task<int> GetLastPageProcessedAsync();

    Task<Item?> FindByIdAsync(int dtoItemId);
    Task<PagedResponse<T>> GetAllPagedAsync<T>(PagedRequest pagedRequest, string? searchString);
}