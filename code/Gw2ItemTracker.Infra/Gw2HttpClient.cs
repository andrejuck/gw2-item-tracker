using System.Text.Json;
using System.Text.Json.Serialization;
using Libs.Api.Models;
using Microsoft.Extensions.Logging;

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
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }

    public async Task<T?> GetAsync<T>(PagedRequest request, string endpoint)
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public async Task<T?> GetAuthAsync<T>(string endpoint, string token)
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
    }
} 