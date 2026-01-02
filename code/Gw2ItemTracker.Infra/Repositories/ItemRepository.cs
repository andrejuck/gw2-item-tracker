using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra.Context;
using Libs.Api.Infra;
using Libs.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gw2ItemTracker.Infra.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly DbContext _dbContext;
    private readonly FilterDefinitionBuilder<Item> _filterBuilder;

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

    public async Task<PagedResponse<T>> GetAllPagedAsync<T>(PagedRequest pagedRequest, string? searchString)
    {
        var aggregatePipeline = new List<BsonDocument>();

        if (!string.IsNullOrEmpty(searchString))
            aggregatePipeline.Add(BuildSearchParam(searchString));

        aggregatePipeline.Add(BuildItemsFacet(pagedRequest));
        aggregatePipeline.Add(BuildProjection(pagedRequest));

        return await _dbContext.Items.Aggregate<PagedResponse<T>>(aggregatePipeline).FirstOrDefaultAsync();
    }

    private BsonDocument BuildProjection(PagedRequest pagedRequest)
    {
        return new BsonDocument("$project", new BsonDocument
        (
            new List<BsonElement>()
            {
                new BsonElement("Result", "$items"),
                new BsonElement("TotalItems", new BsonDocument("$arrayElemAt", new BsonArray()
                {
                    "$totalCount.count",
                    0
                })),
                new BsonElement("PageSize", new BsonDocument("$literal", pagedRequest.PageSize)),
                new BsonElement("CurrentPage", new BsonDocument("$literal", pagedRequest.CurrentPage)),
                new BsonElement("TotalPages", new BsonDocument("$ceil",
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
        return new BsonDocument("$facet", new BsonDocument()
            {
                new BsonElement("items",
                    new BsonArray()
                    {
                        new BsonDocument("$sort", new BsonDocument("Name", 1)),
                        new BsonDocument("$skip", pagedRequest.CurrentPage * pagedRequest.PageSize),
                        new BsonDocument("$limit", pagedRequest.PageSize),
                    }
                ),
                new BsonElement("totalCount",
                    new BsonArray()
                    {
                        new BsonDocument("$count", "count")
                    }
                )
            }
        );
    }

    private BsonDocument BuildSearchParam(string? searchString)
    {
        return new BsonDocument("$match",
            new BsonDocument("Name",
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