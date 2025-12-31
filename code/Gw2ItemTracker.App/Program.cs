using System.Threading.Channels;
using Gw2ItemTracker.App.Adapters;
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

builder.Services.AddScoped<ISynchronizeAdapter, SynchronizeAdapter>();
builder.Services.AddTransient<IGw2HttpClient, Gw2HttpClient>();
builder.Services.AddTransient<IItemAdapter, ItemAdapter>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddSingleton(_ => Channel.CreateUnbounded<ProcessingResource<ItemDto>>());
builder.Services.AddHostedService<ItemService>();

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
