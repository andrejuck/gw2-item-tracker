using System.Text.Json;
using Libs.Api.Models;

namespace Gw2ItemTracker.Infra;

public class Gw2HttpClient : IGw2HttpClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    public Gw2HttpClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.guildwars2.com/");
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            IgnoreNullValues = true,
        };
    }

    public async Task<T?> GetAsync<T>(PagedRequest request, string endpoint)
    {
        var uriBuilder = new UriBuilder(_client.BaseAddress)
        {
            Query = $"page={request.CurrentPage - 1}&page_size={request.PageSize}",
            Path = endpoint 
        };
        
        var response = await _client.GetAsync(uriBuilder.Uri);
        var content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
        
        return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
    }
    
    public async Task<T?> GetAuthAsync<T>(string endpoint, string token)
    {
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        var uriBuilder = new UriBuilder(_client.BaseAddress)
        {
            Path = endpoint
        };
        
        var response = await _client.GetAsync(uriBuilder.Uri);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
        
        return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
    }
} 