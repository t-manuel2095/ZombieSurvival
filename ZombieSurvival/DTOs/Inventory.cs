namespace ZombieSurvival.DTOs
{
    public class InventoryItemResponse
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
