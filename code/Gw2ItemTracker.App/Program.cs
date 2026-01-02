using System.Threading.Channels;
using Gw2ItemTracker.App.Adapters;
using Gw2ItemTracker.App.Application;
using Gw2ItemTracker.App.Helpers;
using Gw2ItemTracker.App.Settings;
using Gw2ItemTracker.Domain.Adapters;
using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra;
using Gw2ItemTracker.Infra.Repositories;
using Gw2ItemTracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mongoConn = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddMongoDbContext(mongoConn.ConnectionString, mongoConn.DbName);

builder.Services.AddTransient<IGw2HttpClient, Gw2HttpClient>();
builder.Services.AddTransient<ISynchronizeApplication, SynchronizeApplication>();
builder.Services.AddTransient<IAccountApplication, AccountApplication>();

builder.Services.AddScoped<ISynchronizeAdapter, SynchronizeAdapter>();
builder.Services.AddTransient<IItemAdapter, ItemAdapter>();
builder.Services.AddTransient<IRecipeAdapter, RecipeAdapter>();
builder.Services.AddTransient<IAccountMaterialAdapter, AccountMaterialAdapter>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();

builder.Services.AddSingleton(_ => Channel.CreateUnbounded<ProcessingResource<ItemDto>>());
builder.Services.AddSingleton(_ => Channel.CreateUnbounded<ProcessingResource<RecipeDto>>());

builder.Services.AddHostedService<ItemService>();
builder.Services.AddHostedService<RecipeService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
