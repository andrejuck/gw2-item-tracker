namespace Gw2ItemTracker.Domain.Models;

public enum ProcessingStatus
{
    None = 0,
    Queued = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4
}