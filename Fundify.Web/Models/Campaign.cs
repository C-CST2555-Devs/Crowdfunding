namespace Fundify.Web.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Goal { get; set; }
        public decimal RaisedAmount { get; set; }
        public int BackerCount { get; set; }
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int DurationInDays { get; set; }
        
        // Add these properties
        public int CreatorId { get; set; }
        public User Creator { get; set; } = null!;
        
        // Add back the Rewards collection
        public List<Reward> Rewards { get; set; } = new();
    }
} 