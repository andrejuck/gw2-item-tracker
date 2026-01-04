namespace Gw2ItemTracker.Domain.Models;

public record Recipe(
    int Id,
    string Type,
    int OutputItemId,
    Item Item,
    int OutputItemCount,
    long TimeToCraftMs,
    IEnumerable<string> Disciplines,
    int MinRating,
    IEnumerable<string> Flags,
    IEnumerable<Ingredient> Ingredients,
    string ChatLink,
    int CurrentPage
);