namespace ZombieSurvival.Models
{
    public class Gun : Item
    {
        public int Damage { get; set; }
        public int Accuracy { get; set; }
        public decimal ReloadSpead { get; set; }
        public int CurrentMagCapacity { get; set; }
        public int MaxMagCapacity { get; set; }
    }
}
