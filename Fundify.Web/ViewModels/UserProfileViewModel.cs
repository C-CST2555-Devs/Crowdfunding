using Fundify.Web.Models;

namespace Fundify.Web.ViewModels
{
    public class UserProfileViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasCompletedOnboarding { get; set; }
        
        // Additional profile information
        public string? Bio { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? WebsiteUrl { get; set; }
        
        // Stats based on account type
        public int? TotalBackedProjects { get; set; }
        public decimal? TotalInvested { get; set; }
        public int? CreatedCampaigns { get; set; }
        public decimal? TotalRaised { get; set; }
    }
} 