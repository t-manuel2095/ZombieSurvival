namespace ZombieSurvival.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public int Hunger { get; set; }
        public int Thirst { get; set; }
        public int Stamina { get; set; }
        public int? locationId { get; set; }
        public Location? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActiveAt { get; set; }
    }
}
