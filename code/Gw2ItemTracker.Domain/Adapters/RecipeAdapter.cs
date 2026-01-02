using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Domain.Adapters;

public class RecipeAdapter : IRecipeAdapter
{
    public Recipe ConvertToDomain(RecipeDto dto, Item recipeItem, int currentPage) =>
        new Recipe(dto.id,
            dto.type,
            dto.output_item_id,
            recipeItem,
            dto.output_item_count,
            dto.time_to_craft_ms,
            dto.disciplines,
            dto.min_rating,
            dto.flags,
            ConvertToDomain(dto.ingredients),
            dto.chat_link,
            currentPage);

    private IEnumerable<Ingredient> ConvertToDomain(IEnumerable<IngredientDto> dtos) =>
        dtos.Select(dto => new Ingredient(dto.id, dto.type, dto.count));
}