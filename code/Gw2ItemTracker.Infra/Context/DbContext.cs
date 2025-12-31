using Gw2ItemTracker.Domain.Models;
using Libs.Api.Infra;
using MongoDB.Driver;

namespace Gw2ItemTracker.Infra.Context;

public class DbContext : MongoDbContext
{
    public IMongoCollection<Item> Items { get; set; }
    public DbContext(string connectionString, string dbName)
        : base(connectionString, dbName)
    {
    }

    protected override void InitializeCollections()
    {
        Items = Database.GetCollection<Item>("items");
    }

    public void InitializeData()
    {

    }

    private FilterDefinition<T> NameFilter<T>(string name)
    {
        return Builders<T>.Filter.Eq("Name", name);
    }
}
