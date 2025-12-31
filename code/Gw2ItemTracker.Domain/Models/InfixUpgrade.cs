using Gw2ItemTracker.Domain.Dto;

namespace Gw2ItemTracker.Domain.Models;

public record InfixUpgrade(
    IEnumerable<InfixAttribute> Attributes
);