using System;

namespace Fundify.Web.Models
{
    public class Pledge
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int? RewardId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public Campaign Campaign { get; set; } = null!;
        public User User { get; set; } = null!;
        public Reward? Reward { get; set; }
    }
} 