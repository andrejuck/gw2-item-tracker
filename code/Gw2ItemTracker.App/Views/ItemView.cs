using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Views;

public record ItemView(
    string Name,
    string Description,
    string Type,
    string Rarity,
    int Id,
    string Icon,
    ItemDetailView? Detail
);

public record ItemDetailView(
    string Type,
    string DamageType,
    string WeightClass,
    int Defense,
    int MinPower,
    int MaxPower,
    InfixUpgradeView? InfixUpgrade,
    int SuffixItemId,
    string SecondarySuffixItemId,
    IEnumerable<int> StatChoices
);

public record InfixUpgradeView(
    IEnumerable<InfixAttributeView> Attributes
);

public record InfixAttributeView(
    string AttributeName,
    int Modifier
);