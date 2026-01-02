using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Domain.DataContracts;

public interface IItemRepository
{
    Task<int> GetLastPageProcessedAsync();

    Task<Item?> FindByIdAsync(int dtoItemId);
}