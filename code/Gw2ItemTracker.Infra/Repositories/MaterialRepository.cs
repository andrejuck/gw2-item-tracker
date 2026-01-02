using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra.Context;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Gw2ItemTracker.Infra.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly DbContext _dbContext;
    private readonly ILogger<MaterialRepository> _logger;
    private readonly FilterDefinitionBuilder<MaterialCategory> _categoryFilterBuilder;
    private readonly FilterDefinitionBuilder<MaterialStorage> _storageFilterBuilder;

    public MaterialRepository(DbContext dbContext, 
        ILogger<MaterialRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        
        _categoryFilterBuilder = Builders<MaterialCategory>.Filter;
        _storageFilterBuilder = Builders<MaterialStorage>.Filter;
    }

    public async Task AddOrUpdateAsync(MaterialCategory materialCategory)
    {
        var idFilter = _categoryFilterBuilder.Eq("_id", materialCategory.Id);
        var replaced = await _dbContext.MaterialCategories.FindOneAndReplaceAsync(idFilter, materialCategory);

        if (replaced is null)
        {
            await _dbContext.MaterialCategories.InsertOneAsync(materialCategory);
            _logger.LogInformation("Material category {MaterialCategoryId} has been created", materialCategory.Id);
        }
        
        _logger.LogInformation("Material category {MaterialCategoryId} has been updated", materialCategory.Id);
    }

    public async Task AddManyAsync(IEnumerable<MaterialStorage> materialStorageList)
    {
        await _dbContext.MaterialStorage.InsertManyAsync(materialStorageList);
    }

    public async Task<MaterialCategory?> FindCategoryByIdAsync(int dtoCategoryId)
    {
        var idFilter = _categoryFilterBuilder.Eq("_id", dtoCategoryId);
        return await _dbContext.MaterialCategories.Find(idFilter).FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<MaterialStorage>> GetAllStorageAsync()
    {
        return await _dbContext.MaterialStorage.Find(_storageFilterBuilder.Empty).ToListAsync();
    }
}