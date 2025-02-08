public class CheckoutViewModel
{
    public int CampaignId { get; set; }
    public string CampaignTitle { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int? RewardId { get; set; }
    public string? RewardName { get; set; }
    public string? RewardDescription { get; set; }
    public decimal AvailableBalance { get; set; }
} 