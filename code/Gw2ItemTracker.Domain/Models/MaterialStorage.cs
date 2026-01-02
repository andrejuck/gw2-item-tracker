namespace Gw2ItemTracker.Domain.Models;

public record MaterialStorage(
    int ItemId,
    string ItemName,
    string ItemIcon,
    int CategoryId,
    string CategoryName
);