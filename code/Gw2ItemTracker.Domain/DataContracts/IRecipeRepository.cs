using Gw2ItemTracker.Domain.Models;
using Libs.Api.Models;

namespace Gw2ItemTracker.Domain.DataContracts;

public interface IRecipeRepository
{
    Task<int> GetLastPageProcessedAsync();
    Task<Recipe?> FindByIdAsync(int itemId);
    Task<PagedResponse<T>> GetAllPagedAsync<T>(PagedRequest pagedRequest, string? searchString);
}