namespace Fundify.Api.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Goal { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int DurationInDays { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal RaisedAmount { get; set; }
        public int BackerCount { get; set; }
        public List<Reward> Rewards { get; set; } = new();
    }
} 