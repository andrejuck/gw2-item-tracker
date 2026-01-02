using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra.Context;
using Libs.Api.Infra;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gw2ItemTracker.Infra.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly DbContext _dbContext;
    private FilterDefinitionBuilder<Item> _filterBuilder;

    public ItemRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _filterBuilder = Builders<Item>.Filter;
    }

    public async Task<int> GetLastPageProcessedAsync()
    {
        var sortPipeline = new BsonDocument("$sort", new BsonDocument("CurrentPage", -1));
        var projectionPipeline = new BsonDocument("$project", new BsonDocument("CurrentPage", 1));
        var limitPipeline = new BsonDocument("$limit", 1);
        var aggregatePipeline = new[]
        {
            sortPipeline,
            projectionPipeline,
            limitPipeline
        };

        var aggregate = await _dbContext.Items.AggregateAsync<BsonDocument>(aggregatePipeline);
        var result = await aggregate.FirstOrDefaultAsync();

        return result["CurrentPage"].AsInt32;

    }

    public async Task<Item?> FindByIdAsync(int dtoItemId)
    {
        var idFilter = _filterBuilder.Eq(x => x.Id, dtoItemId);
        return await _dbContext.Items.Find(idFilter).FirstOrDefaultAsync();
    }
}