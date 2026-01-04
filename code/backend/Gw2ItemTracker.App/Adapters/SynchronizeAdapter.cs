using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Adapters;

public class SynchronizeAdapter : ISynchronizeAdapter
{
    public IEnumerable<ProcessingResource<ItemDto>> ConvertToProcessingResource(IEnumerable<ItemDto> dtos,
        int currentPage) =>
        dtos.Select(x => ConvertToProcessingResource(x, currentPage));

    public IEnumerable<ProcessingResource<RecipeDto>> ConvertToProcessingResource(IEnumerable<RecipeDto> dtos,
        int currentPage) =>
        dtos.Select(dto => ConvertToProcessingResource(dto, currentPage));

    public IEnumerable<ProcessingResource<MaterialCategoryDto>> ConvertToProcessingResource(IEnumerable<MaterialCategoryDto> dtos,
        int currentPage) =>
        dtos.Select(dto => ConvertToProcessingResource(dto, currentPage));

    private ProcessingResource<RecipeDto> ConvertToProcessingResource(RecipeDto dto, int currentPage) =>
        new(dto.id, "recipes", ProcessingStatus.Queued, dto, currentPage);

    private ProcessingResource<ItemDto> ConvertToProcessingResource(ItemDto item, int currentPage)
        => new(item.id, "items", ProcessingStatus.Queued, item, currentPage);

    private ProcessingResource<MaterialCategoryDto> ConvertToProcessingResource(MaterialCategoryDto dto,
        int currentPage) =>
        new(dto.id, "materials", ProcessingStatus.Queued, dto, currentPage);
}