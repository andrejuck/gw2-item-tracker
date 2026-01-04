namespace Gw2ItemTracker.Domain.Models;

public class CharacterDto
{
    public string name { get; set; }
    public string race { get; set; }
    public string gender { get; set; }
    public string profession { get; set; }
    public int level { get; set; }
    public int age { get; set; }
    public string created { get; set; }
    public int deaths { get; set; }
    public int title { get; set; }
    public BagsDto[] bags { get; set; }
}

public class BagsDto
{
    public int id { get; set; }
    public int size { get; set; }
    public InventoryDto[] inventory { get; set; }
}

public class InventoryDto
{
    public int id { get; set; }
    public int count { get; set; }
    public int charges { get; set; }
}