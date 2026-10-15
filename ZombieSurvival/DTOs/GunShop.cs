namespace ZombieSurvival.DTOs
{
    public class BuyGunRequest
    {
        public int PlayerId { get; set; }
        public int GunId { get; set; }
    }
    public class GunShopItemResponse
    {
        public int GunId { get; set; }
        public string GunName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Damage { get; set; }
        public int Accuracy { get; set; }
        public decimal ReloadSpead { get; set; }
        public int CurrentMagCapacity { get; set; }
        public int MaxMagCapacity { get; set; }
        public bool AlreadyOwned { get; set; }
    }

    public class BuyGunResponse
    {
        public string GunName { get; set; } = string.Empty;
        public int PricePaid { get; set; }
        public int Money { get; set; }

    }

    public class SellItemRequest
    {
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class SellItemResponse
    {
        public string ItemName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public int MoneyEarned { get; set; }
        public int Money { get; set; }
    }
}
