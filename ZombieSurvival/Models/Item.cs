namespace ZombieSurvival.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public int MaxStackSize { get; set; }
        public int Price { get; set; }

    }
}
