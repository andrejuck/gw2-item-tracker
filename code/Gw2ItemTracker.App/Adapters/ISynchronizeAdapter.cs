using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Adapters;

public interface ISynchronizeAdapter
{
    IEnumerable<ProcessingResource<ItemDto>> ConvertToProcessingResource(IEnumerable<ItemDto> items, int currentPage);
}