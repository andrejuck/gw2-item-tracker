using Gw2ItemTracker.App.Adapters;
using Gw2ItemTracker.App.Views;
using Gw2ItemTracker.Domain.DataContracts;
using Libs.Api.Models;

namespace Gw2ItemTracker.App.Application;

public class RecipeApplication : IRecipeApplication
{
    private readonly IRecipeAdapter _adapter;
    private readonly IRecipeRepository _repository;
    private readonly ILogger<RecipeApplication> _logger;

    public RecipeApplication(IRecipeAdapter adapter,
        IRecipeRepository repository, 
        ILogger<RecipeApplication> logger)
    {
        _adapter = adapter;
        _repository = repository;
        _logger = logger;
    }

    public async Task<PagedResponse<RecipeView>> GetPagedAsync(PagedRequest pagedRequest, string? searchString)
    {
        return await _repository.GetAllPagedAsync<RecipeView>(pagedRequest, searchString);
    }

    public async Task<RecipeView?> GetByIdAsync(int recipeId)
    {
        var entity = await _repository.FindByIdAsync(recipeId);
        if (entity is null)
        {
            _logger.LogError($"Recipe with id {recipeId} not found");
            return null;
        }
        
        _logger.LogInformation($"Recipe recipe with id {recipeId} found");
        return _adapter.ConvertToView(entity);
    }
}