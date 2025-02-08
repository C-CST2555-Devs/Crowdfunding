using System;
using System.Collections.Generic;
using Fundify.Web.ViewModels;

namespace Fundify.Web.ViewModels
{
    public class CampaignDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Goal { get; set; }
        public decimal RaisedAmount { get; set; }
        public int BackerCount { get; set; }
        public int DurationInDays { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<RewardViewModel> Rewards { get; set; } = new();
        public List<CommentViewModel> Comments { get; set; } = new();
        public List<SupporterViewModel> Supporters { get; set; } = new();

        public int DaysLeft => Math.Max(0, DurationInDays - (int)(DateTime.UtcNow - CreatedAt).TotalDays);
        public decimal ProgressPercentage => Goal > 0 ? Math.Min((RaisedAmount / Goal * 100), 100) : 0;
        public bool IsActive => DaysLeft > 0;
        public DateTime EndDate => CreatedAt.AddDays(DurationInDays);
    }

    public class CommentViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class SupporterViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PledgedAt { get; set; }
    }
} 