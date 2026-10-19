namespace ZombieSurvival.DTOs
{
    public class UseItemRequest
    {
        public int PlayerId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; } = 1;

    }

    public class UseItemRespone
    {
        public string ItemName { get; set; } = string.Empty;
        public int Hunger { get; set; }
        public int Thirst { get; set; }
        public int Stamina { get; set; }
        public int? CarCondition { get; set; }
        public string Message { get; set; } = string.Empty;

    }

    public class EndMorningRequest
    {
        public int PlayerId { get; set; }
    }
}
