namespace Gw2ItemTracker.Domain.Models;

public class ProcessingResource<T>(long id, string endpoint, ProcessingStatus status, T? resource, int currentPage)
{
    public long Id { get; set; } = id;
    public string Endpoint { get; set; } = endpoint;
    public ProcessingStatus Status { get; private set; } = status;
    public T? Resource { get; private set; } = resource;
    public int CurrentPage { get; private set; } = currentPage;

    public void StartProcessing()
    {
        Status = ProcessingStatus.Processing;
    }

    public void CompleteProcessing()
    {
        Status = ProcessingStatus.Completed;
    }

    public void FailProcessing()
    {
        Status = ProcessingStatus.Failed;
    }
};