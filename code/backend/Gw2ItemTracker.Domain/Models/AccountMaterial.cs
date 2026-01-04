namespace Gw2ItemTracker.Domain.Models;

public record AccountMaterial(
    int ItemId,
    int CategoryId,
    int Count
);