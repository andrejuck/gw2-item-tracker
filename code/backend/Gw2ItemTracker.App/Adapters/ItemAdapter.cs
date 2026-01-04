using Gw2ItemTracker.App.Views;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Adapters;

public class ItemAdapter : IItemAdapter
{
    public ItemView ConvertToView(Item domain)
        => new ItemView(domain.Name,
            domain.Description,
            domain.Type,
            domain.Rarity,
            domain.Id,
            domain.Icon,
            ConvertToView(domain.Detail));


    private ItemDetailView? ConvertToView(ItemDetail? domain)
        =>
            domain is null
                ? null
                : new(
                    domain.Type,
                    domain.DamageType,
                    domain.WeightClass,
                    domain.Defense,
                    domain.MinPower,
                    domain.MaxPower,
                    ConvertToView(domain.InfixUpgrade),
                    domain.SuffixItemId,
                    domain.SecondarySuffixItemId,
                    domain.StatChoices
                );

    private static InfixUpgradeView? ConvertToView(InfixUpgrade? dto)
    {
        return dto is null
            ? null
            : new InfixUpgradeView(dto.Attributes.Select(x => new InfixAttributeView(x.AttributeName, x.Modifier)));
    }
}