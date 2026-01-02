using Gw2ItemTracker.App.Views;
using Libs.Api.Models;

namespace Gw2ItemTracker.App.Application;

public interface IRecipeApplication
{
    Task<PagedResponse<RecipeView>> GetPagedAsync(PagedRequest pagedRequest, string? searchString);
    Task<RecipeView?> GetByIdAsync(int recipeId);
}