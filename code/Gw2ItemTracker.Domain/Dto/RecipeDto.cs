namespace Gw2ItemTracker.Domain.Dto;

public class RecipeDto
{
    public int id { get; set; }
    public string type { get; set; }
    public int output_item_id { get; set; }
    public int output_item_count { get; set; }
    public int time_to_craft_ms { get; set; }
    public string[] disciplines { get; set; }
    public int min_rating { get; set; }
    public string[] flags { get; set; }
    public IngredientDto[] ingredients { get; set; }
    public string chat_link { get; set; }
}