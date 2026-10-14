namespace ZombieSurvival.DTOs
{
    public class ScavengeRequest
    {
        public int PlayerId { get; set; }
        // Area the player chose to loot (must be in their current city)
        public int LocationId { get; set; }
    }

    public class ScavengeResponse
    {
        public int Stamina { get; set; }
        public string? ItemName { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}