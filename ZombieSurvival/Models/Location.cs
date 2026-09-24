namespace ZombieSurvival.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LocationType { get; set; } = string.Empty;
        public int DangerLevel { get; set; }
    }
}
