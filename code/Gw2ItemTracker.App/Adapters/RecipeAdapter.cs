using Gw2ItemTracker.App.Views;
using Gw2ItemTracker.Domain.DataContracts;
using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Adapters;

public class RecipeAdapter : IRecipeAdapter
{
    private readonly IItemAdapter _itemAdapter;

    public RecipeAdapter(IItemAdapter itemAdapter)
    {
        _itemAdapter = itemAdapter;
    }

    public RecipeView ConvertToView(Recipe entity) =>
        new RecipeView(
            entity.Id,
            entity.Type,
            entity.OutputItemId,
            _itemAdapter.ConvertToView(entity.Item),
            entity.OutputItemCount,
            entity.TimeToCraftMs,
            entity.Disciplines,
            entity.MinRating,
            entity.Ingredients.Select(x => new IngredientView(x.Id, x.Type, x.Count)),
            entity.ChatLink
        );
}