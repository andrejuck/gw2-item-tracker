namespace Gw2ItemTracker.App.Views;

public record RecipeView(
    int Id,
    string Type,
    int OutputItemId,
    ItemView Item,
    int OutputItemCount,
    long TimeToCraftMs,
    IEnumerable<string> Disciplines,
    int MinRating,
    IEnumerable<IngredientView> Ingredients,
    string ChatLink
);

public record IngredientView(int Id, string Type, int Count);