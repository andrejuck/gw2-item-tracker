using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Applications;

public interface ISynchronizeApplication
{
    IEnumerable<ProcessingResource> ConvertToProcessingResource(IEnumerable<int> items);
}