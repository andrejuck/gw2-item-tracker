namespace Gw2ItemTracker.Domain.DataContracts;

public interface IItemRepository
{
    Task<int> GetLastPageProcessedAsync();

}