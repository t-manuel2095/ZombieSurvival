namespace ZombieSurvival.Models
{
    public class Gun
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        public int Accuracy { get; set; }
    }
}
