using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Domain.Adapters;

public interface IRecipeAdapter
{
    Recipe ConvertToDomain(RecipeDto dto, Item recipeItem, int currentPage);
}