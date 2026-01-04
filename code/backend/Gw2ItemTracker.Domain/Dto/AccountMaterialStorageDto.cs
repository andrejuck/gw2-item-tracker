namespace Gw2ItemTracker.Domain.Dto;

public record AccountMaterialStorageDto(
    int ItemId,
    int CategoryId,
    int Count,
    string ItemName,
    string ItemIcon,
    string CategoryName
);