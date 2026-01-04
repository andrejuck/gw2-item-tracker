using Gw2ItemTracker.Domain.Dto;
using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Adapters;

public interface ISynchronizeAdapter
{
    IEnumerable<ProcessingResource<ItemDto>> ConvertToProcessingResource(IEnumerable<ItemDto> dtos, int currentPage);

    IEnumerable<ProcessingResource<RecipeDto>>
        ConvertToProcessingResource(IEnumerable<RecipeDto> dtos, int currentPage);

    IEnumerable<ProcessingResource<MaterialCategoryDto>> ConvertToProcessingResource(
        IEnumerable<MaterialCategoryDto> dtos,
        int currentPage);
}