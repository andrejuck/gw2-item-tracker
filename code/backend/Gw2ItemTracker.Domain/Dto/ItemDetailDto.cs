namespace Gw2ItemTracker.Domain.Dto;

public class ItemDetailDto
{
    public string type { get; set; }
    public string damage_type { get; set; }
    public string weight_class { get; set; }
    public int defense { get; set; }
    public int min_power { get; set; }
    public int max_power { get; set; }
    public object[] infusion_slots { get; set; }
    public InfixUpgradeDto infix_upgrade { get; set; }
    public int suffix_item_id { get; set; }
    public string secondary_suffix_item_id { get; set; }
    public int[] stat_choices { get; set; }
}