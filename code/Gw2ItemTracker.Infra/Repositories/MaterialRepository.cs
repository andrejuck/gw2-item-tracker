using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra.Context;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Gw2ItemTracker.Infra.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly DbContext _dbContext;
    private readonly ILogger<MaterialRepository> _logger;
    private FilterDefinitionBuilder<MaterialCategory> _filterBuilder;

    public MaterialRepository(DbContext dbContext, 
        ILogger<MaterialRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        
        _filterBuilder = Builders<MaterialCategory>.Filter;
    }

    public async Task AddOrUpdateAsync(MaterialCategory materialCategory)
    {
        var idFilter = _filterBuilder.Eq("_id", materialCategory.Id);
        var replaced = await _dbContext.MaterialCategories.FindOneAndReplaceAsync(idFilter, materialCategory);

        if (replaced is null)
        {
            await _dbContext.MaterialCategories.InsertOneAsync(materialCategory);
            _logger.LogInformation($"Material category {materialCategory.Id} has been created");
        }
        
        _logger.LogInformation($"Material category {materialCategory.Id} has been updated");
    }
}