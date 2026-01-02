using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra.Context;
using Libs.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gw2ItemTracker.Infra.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly DbContext _dbContext;
    private readonly FilterDefinitionBuilder<Recipe> _filterBuilder;

    public RecipeRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _filterBuilder = Builders<Recipe>.Filter;
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

    public async Task<Recipe?> FindByIdAsync(int dtoItemId)
    {
        var idFilter = _filterBuilder.Eq(x => x.Id, dtoItemId);
        return await _dbContext.Recipes.Find(idFilter).FirstOrDefaultAsync();
    }

    //THis method should be put in Juck.Libs.Api
    public async Task<PagedResponse<T>> GetAllPagedAsync<T>(PagedRequest pagedRequest, string? searchString)
    {
        var aggregatePipeline = new List<BsonDocument>();

        if (!string.IsNullOrEmpty(searchString))
            aggregatePipeline.Add(BuildSearchParam("Item.Name", searchString));

        aggregatePipeline.Add(BuildItemsFacet(pagedRequest));
        aggregatePipeline.Add(BuildProjection(pagedRequest));

        return await _dbContext.Recipes.Aggregate<PagedResponse<T>>(aggregatePipeline).FirstOrDefaultAsync();
    }

    private BsonDocument BuildProjection(PagedRequest pagedRequest)
    {
        return new BsonDocument("$project", new BsonDocument
        (
            new List<BsonElement>()
            {
                new("Result", "$items"),
                new("TotalItems", new BsonDocument("$arrayElemAt", new BsonArray()
                {
                    "$totalCount.count",
                    0
                })),
                new("PageSize", new BsonDocument("$literal", pagedRequest.PageSize)),
                new("CurrentPage", new BsonDocument("$literal", pagedRequest.CurrentPage)),
                new("TotalPages", new BsonDocument("$ceil",
                        new BsonDocument("$divide", new BsonArray()
                        {
                            new BsonDocument("$arrayElemAt", new BsonArray()
                            {
                                "$totalCount.count",
                                0,
                            }),
                            pagedRequest.PageSize
                        })
                    )
                )
            }
        ));
    }

    private BsonDocument BuildItemsFacet(PagedRequest pagedRequest)
    {
        return new BsonDocument("$facet", new BsonDocument
            (
                new List<BsonElement>()
                {
                    new("items",
                        new BsonArray()
                        {
                            new BsonDocument("$sort", new BsonDocument(pagedRequest.SortKey, 1)),
                            new BsonDocument("$skip", pagedRequest.CurrentPage * pagedRequest.PageSize),
                            new BsonDocument("$limit", pagedRequest.PageSize),
                        }
                    ),
                    new("totalCount",
                        new BsonArray()
                        {
                            new BsonDocument("$count", "count")
                        }
                    )
                }
            )
        );
    }

    private BsonDocument BuildSearchParam(string propName, string? searchString)
    {
        return new BsonDocument("$match",
            new BsonDocument(propName,
                new BsonDocument(
                    new List<BsonElement>()
                    {
                        new("$regex", searchString),
                        new("$options", "i")
                    }
                )
            )
        );
    }
}