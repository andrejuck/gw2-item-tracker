namespace Gw2ItemTracker.App.Application;

public interface ISynchronizeApplication
{
    Task<bool> SynchronizeAllItemsAsync(bool synchronizeAll);
    Task<bool> SynchronizeAllRecipesAsync(bool synchronizeAll);
    Task<bool> SynchronizeMaterialCategoriesAsync();
}