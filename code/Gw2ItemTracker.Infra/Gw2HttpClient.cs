using System.Text.Json;
using Libs.Api.Models;

namespace Gw2ItemTracker.Infra;

public class Gw2HttpClient : IGw2HttpClient
{
    private readonly HttpClient _client;
    public Gw2HttpClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.guildwars2.com/v2/");
    }

    public async Task<T?> GetAsync<T>(PagedRequest request, string endpoint)
    {
        var response = await _client.GetAsync(endpoint);
        var content = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<T>(content);
    }
} 