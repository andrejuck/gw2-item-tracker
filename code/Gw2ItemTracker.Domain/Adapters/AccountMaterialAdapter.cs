using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Domain.Adapters;

public class AccountMaterialAdapter : IAccountMaterialAdapter
{
    public AccountMaterialStorageDto ConvertToDto(AccountMaterialDto dto,
        MaterialStorage matStorage)
        => new(dto.ItemId, 
            dto.CategoryId, 
            dto.Count,
            matStorage.ItemName,
            matStorage.ItemIcon,
            matStorage.CategoryName);

    public MaterialStorage ConvertToStorageDomain(AccountMaterialDto dto, Item item, MaterialCategory category)
        => new(dto.ItemId, item.Name, item.Icon, dto.CategoryId, category.Name);
}