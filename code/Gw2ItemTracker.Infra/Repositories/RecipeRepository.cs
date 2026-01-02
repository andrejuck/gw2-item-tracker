using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Infra.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gw2ItemTracker.Infra.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly DbContext _dbContext;

    public RecipeRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
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

        var aggregate = await _dbContext.Recipes.AggregateAsync<BsonDocument>(aggregatePipeline);
        var result = await aggregate.FirstOrDefaultAsync();

        return result["CurrentPage"].AsInt32;
    }
}