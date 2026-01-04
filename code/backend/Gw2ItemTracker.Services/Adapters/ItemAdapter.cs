using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Services.Adapters;

public static class ItemAdapter
{
    public static Item ConvertToDomain(ItemDto itemDto, int currentPage) =>
        new Item(
            itemDto.name,
            itemDto.description,
            itemDto.type,
            itemDto.rarity,
            itemDto.vendor_value,
            itemDto.restrictions,
            itemDto.id,
            itemDto.icon,
            ConvertToDomain(itemDto.details),
            currentPage
        );
    
    private static ItemDetail? ConvertToDomain(ItemDetailDto? dto)
    {
        return dto is null
            ? null
            : new ItemDetail(
                dto.type,
                dto.damage_type,
                dto.weight_class,
                dto.defense,
                dto.min_power,
                dto.max_power,
                ConvertToDomain(dto.infix_upgrade),
                dto.suffix_item_id,
                dto.secondary_suffix_item_id,
                dto.stat_choices
            );
    }


    private static InfixUpgrade? ConvertToDomain(InfixUpgradeDto? dto)
    {
        return dto is null
            ? null
            : new InfixUpgrade(dto.attributes.Select(x => new InfixAttribute(x.attribute, x.modifier)));
    }
}