namespace Gw2ItemTracker.Domain.DataContracts;

public interface IRecipeRepository
{
    Task<int> GetLastPageProcessedAsync();
}