using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Domain.Adapters;

public interface IItemAdapter
{
    Item ConvertToDomain(ItemDto itemDto, int currentPage);
}