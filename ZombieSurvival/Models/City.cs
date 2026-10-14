namespace ZombieSurvival.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RouteOrder { get; set; }
        public bool IsWinCity { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();
    }
}
