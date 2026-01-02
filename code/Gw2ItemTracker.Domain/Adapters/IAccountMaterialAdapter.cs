using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Domain.Adapters;

public interface IAccountMaterialAdapter
{
    AccountMaterialStorageDto ConvertToDto(AccountMaterialDto dto, MaterialStorage matStorage);

    MaterialStorage ConvertToStorageDomain(AccountMaterialDto dto, Item item, MaterialCategory category);
}