using Libs.Api.Models;

namespace Gw2ItemTracker.Infra;

public interface IGw2HttpClient
{
    Task<T?> GetAsync<T>(PagedRequest request, string endpoint);
}