using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Adapters;

public class SynchronizeAdapter : ISynchronizeAdapter
{
    public IEnumerable<ProcessingResource<ItemDto>> ConvertToProcessingResource(IEnumerable<ItemDto> items, int currentPage) =>
        items.Select(x => ConvertToProcessingResource(x, currentPage));

    private ProcessingResource<ItemDto> ConvertToProcessingResource(ItemDto item, int currentPage)
        => new ProcessingResource<ItemDto>(item.id, "items", ProcessingStatus.Queued, item, currentPage);
}
