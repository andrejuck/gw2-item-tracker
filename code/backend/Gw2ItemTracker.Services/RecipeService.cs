using System.Threading.Channels;
using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra.Context;
using Gw2ItemTracker.Services.Adapters;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Gw2ItemTracker.Services;

public class RecipeService : BackgroundService
{
    private readonly Channel<ProcessingResource<RecipeDto>> _recipeDtoChannel;
    private readonly DbContext _dbContext;

    public RecipeService(
        Channel<ProcessingResource<RecipeDto>> recipeDtoChannel,
        DbContext dbContext
    )
    {
        _recipeDtoChannel = recipeDtoChannel;
        _dbContext = dbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (await _recipeDtoChannel.Reader.WaitToReadAsync(stoppingToken))
            {
                if (!_recipeDtoChannel.Reader.TryRead(out var recipeDto))
                    continue;

                try
                {
                    recipeDto.StartProcessing();
                    Console.WriteLine($"Processing recipe {recipeDto.Id}");
                    
                    var recipeItem = await FindRecipeItemAsync(stoppingToken, recipeDto);
                    if (recipeItem is null)
                    {
                        Console.WriteLine($"Item {recipeDto.Resource.output_item_id} not found for recipe {recipeDto.Id}");
                        recipeDto.FailProcessing();
                        continue;
                    }
                    
                    var recipeIngredientsItem = await FindRecipeIngredientsAsync(stoppingToken, recipeDto);
                    await AddOrUpdateRecipeAsync(stoppingToken, recipeDto, recipeItem, recipeIngredientsItem);

                    recipeDto.CompleteProcessing();
                }
                catch (OperationCanceledException e) when (stoppingToken.IsCancellationRequested)
                {
                    recipeDto.FailProcessing();
                    Console.WriteLine(e);
                    continue;
                }
                catch (Exception e)
                {
                    recipeDto.FailProcessing();
                    Console.WriteLine(e);
                    continue;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<IEnumerable<Item>> FindRecipeIngredientsAsync(CancellationToken stoppingToken, ProcessingResource<RecipeDto> recipeDto)
    {
        var idsFilter = Builders<Item>.Filter.In("_id", recipeDto.Resource.ingredients.Select(x => x.item_id));
        var ingredientDtoList = await _dbContext.Items.Find(idsFilter).ToListAsync(stoppingToken);
        return ingredientDtoList;
    }

    private async Task AddOrUpdateRecipeAsync(CancellationToken stoppingToken,
        ProcessingResource<RecipeDto> recipeDto,
        Item recipeItem, 
        IEnumerable<Item> ingredients)
    {
        var idFilter = Builders<Recipe>.Filter.Eq("_id", recipeDto.Id);
        var entity = RecipeAdapter.ConvertToDomain(recipeDto.Resource, recipeItem, recipeDto.CurrentPage, ingredients);
        var replaced = await _dbContext.Recipes.FindOneAndReplaceAsync<Recipe>(idFilter,
            entity,
            cancellationToken: stoppingToken);

        if (replaced is null)
            await _dbContext.Recipes.InsertOneAsync(entity, cancellationToken: stoppingToken);
    }

    private async Task<Item?> FindRecipeItemAsync(CancellationToken stoppingToken, ProcessingResource<RecipeDto> recipeDto)
    {
        var itemIdFilter = Builders<Item>.Filter.Eq("_id", recipeDto.Resource.output_item_id); 
        var recipeItem = await _dbContext.Items.Find(itemIdFilter)
            .FirstOrDefaultAsync<Item>(stoppingToken);
        return recipeItem;
    }
}