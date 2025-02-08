using System.ComponentModel.DataAnnotations;
using Fundify.Web.Models;

namespace Fundify.Web.ViewModels
{
    public class OnboardingViewModel
    {
        [Required(ErrorMessage = "Please select an account type")]
        public Models.AccountType AccountType { get; set; }

        // Backer specific fields
        public List<string> InterestCategories { get; set; } = new();
        public decimal? MonthlyInvestmentBudget { get; set; }

        // Creator specific fields
        public string? CompanyName { get; set; }
        public string? ProjectCategory { get; set; }
        public string? ProjectDescription { get; set; }

        // Common fields
        public string? Bio { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? WebsiteUrl { get; set; }
    }
} 