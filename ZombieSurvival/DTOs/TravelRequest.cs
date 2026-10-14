namespace ZombieSurvival.DTOs
{
    public class TravelRequest
    {
        public int PlayerId { get; set; }
        public int LocationId { get; set; }
    }

    public class TravelResponse
    {
        public int PlayerID { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public int Stamina { get; set; }
        public string Message { get; set; } = string.Empty;
    }

}
