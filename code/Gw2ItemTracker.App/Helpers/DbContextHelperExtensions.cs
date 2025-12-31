using Gw2ItemTracker.Infra.Context;
using MongoDB.Bson.Serialization.Conventions;

namespace Gw2ItemTracker.App.Helpers;

public static class DbContextHelperExtensions
{
    public static IServiceCollection AddMongoDbContext(this IServiceCollection services, 
        string connectionString,
        string dbName)
    {
        RegisterMongoSerializers();

        var dbContext = new DbContext(connectionString, dbName);
        services.AddSingleton(dbContext);

        dbContext.InitializeData();

        return services;
    }

    private static void RegisterMongoSerializers()
    {
        var conventionPack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true)
        };
        ConventionRegistry.Register("IgnoreExtraElements", conventionPack, t => true);
    }
}