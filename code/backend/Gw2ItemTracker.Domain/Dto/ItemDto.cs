namespace Gw2ItemTracker.Domain.Dto;

public class ItemDto
{
    public string name { get; set; }
    public string description { get; set; }
    public string type { get; set; }
    public string rarity { get; set; }
    public int vendor_value { get; set; }
    public string[] restrictions { get; set; }
    public int id { get; set; }
    public string icon { get; set; }
    public ItemDetailDto? details { get; set; }
}