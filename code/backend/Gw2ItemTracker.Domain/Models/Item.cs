namespace Gw2ItemTracker.Domain.Models;

public record Item(
    string Name,
    string Description,
    string Type,
    string Rarity,
    int VendorValue,
    IEnumerable<string> Restrictions,
    int Id,
    string Icon,
    ItemDetail? Detail,
    int CurrentPage
);