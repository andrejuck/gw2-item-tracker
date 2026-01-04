using Gw2ItemTracker.App.Adapters;
using Gw2ItemTracker.App.Views;
using Gw2ItemTracker.Domain.DataContracts;
using Libs.Api.Models;

namespace Gw2ItemTracker.App.Application;

public class RecipeApplication : IRecipeApplication
{
    private readonly IRecipeAdapter _adapter;
    private readonly IRecipeRepository _repository;

    public RecipeApplication(IRecipeAdapter adapter,
        IRecipeRepository repository)
    {
        _adapter = adapter;
        _repository = repository;
    }

    public async Task<PagedResponse<RecipeView>> GetPagedAsync(PagedRequest pagedRequest, string? searchString)
    {
        return await _repository.GetAllPagedAsync<RecipeView>(pagedRequest, searchString);
    }

    public async Task<RecipeView?> GetByIdAsync(int recipeId)
    {
        var entity = await _repository.FindByIdAsync(recipeId);
        return entity is null ? null : _adapter.ConvertToView(entity);
    }
}