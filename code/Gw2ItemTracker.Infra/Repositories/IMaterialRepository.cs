using Gw2ItemTracker.Domain.Models;

namespace Gw2ItemTracker.Infra.Repositories;

public interface IMaterialRepository
{
    Task AddOrUpdateAsync(MaterialCategory materialCategory);
}