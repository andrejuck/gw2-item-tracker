using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.App.Applications;

public class SynchronizeApplication : ISynchronizeApplication
{
    public IEnumerable<ProcessingResource> ConvertToProcessingResource(IEnumerable<int> items) =>
        items.Select(ConvertToProcessingResource);

    private ProcessingResource ConvertToProcessingResource(int id)
        => new ProcessingResource(id, "items", ProcessingStatus.Queued);
}
