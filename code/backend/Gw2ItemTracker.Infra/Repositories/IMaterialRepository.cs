using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Infra.Repositories;

public interface IMaterialRepository
{
    Task AddOrUpdateAsync(MaterialCategory materialCategory);
    Task AddManyAsync(IEnumerable<MaterialStorage> materialStorageList);
    Task<MaterialCategory?> FindCategoryByIdAsync(int dtoCategoryId);
    Task<IEnumerable<MaterialStorage>> GetAllStorageAsync();
}