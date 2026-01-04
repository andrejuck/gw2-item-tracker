using System.Text.Json.Serialization;

namespace Gw2ItemTracker.Domain.Dto;

public class AccountMaterialDto
{
    [JsonPropertyName("id")]
    public int ItemId { get; set; }
    [JsonPropertyName("category")]
    public int CategoryId { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
}