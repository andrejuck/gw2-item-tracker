using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Services.Adapters;

public static class RecipeAdapter
{
    public static Recipe ConvertToDomain(RecipeDto dto,
        Item recipeItem,
        int currentPage,
        IEnumerable<Item> ingredients) =>
        new Recipe(dto.id,
            dto.type,
            dto.output_item_id,
            recipeItem,
            dto.output_item_count,
            dto.time_to_craft_ms,
            dto.disciplines,
            dto.min_rating,
            dto.flags,
            ConvertToDomain(dto.ingredients, ingredients),
            dto.chat_link,
            currentPage);

    private static IEnumerable<Ingredient> ConvertToDomain(IEnumerable<IngredientDto> dtos, IEnumerable<Item> ingredientsInfo) =>
        dtos.Select(dto => new Ingredient(dto.item_id, dto.type, dto.count, ingredientsInfo.FirstOrDefault(x => x.Id.Equals(dto.item_id))?.Name));
}