namespace Fundify.Web.Models
{
    public class Reward
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? Limit { get; set; }
        public DateTime EstimatedDelivery { get; set; }
        public Campaign Campaign { get; set; } = null!;
    }
} 