
namespace Gw2ItemTracker.Domain.Models;

public record ItemDetail(
    string Type,
    string DamageType,
    string WeightClass,
    int Defense,
    int MinPower,
    int MaxPower,
    InfixUpgrade? InfixUpgrade,
    int SuffixItemId,
    string SecondarySuffixItemId,
    IEnumerable<int> StatChoices
);