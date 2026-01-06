using System.Threading.Channels;
using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;
using Gw2ItemTracker.Infra.Context;
using Gw2ItemTracker.Services.Adapters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Gw2ItemTracker.Services;

public class RecipeService : BackgroundService
{
    private readonly Channel<ProcessingResource<RecipeDto>> _recipeDtoChannel;
    private readonly DbContext _dbContext;
    private readonly ILogger<RecipeService> _logger;

    public RecipeService(
        Channel<ProcessingResource<RecipeDto>> recipeDtoChannel,
        DbContext dbContext, ILogger<RecipeService> logger)
    {
        _recipeDtoChannel = recipeDtoChannel;
        _dbContext = dbContext;
        _logger = logger;
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
                    _logger.LogInformation($"Processing recipe {recipeDto.Id}");
                    
                    var recipeItem = await FindRecipeItemAsync(stoppingToken, recipeDto);
                    if (recipeItem is null)
                    {
                        _logger.LogError($"Item {recipeDto.Resource.output_item_id} not found for recipe {recipeDto.Id}");
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
                    _logger.LogError("Operation cancelled {e}", e);
                    continue;
                }
                catch (Exception e)
                {
                    recipeDto.FailProcessing();
                    _logger.LogError("An error occurred while processing recipe {e}", e);
                    continue;
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred while reading from recipe queue {e}", e);
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