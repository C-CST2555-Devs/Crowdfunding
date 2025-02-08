using System.ComponentModel.DataAnnotations;

namespace Fundify.Web.Models
{
    public enum AccountType
    {
        Backer,
        Creator,
        Administrator
    }

    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public AccountType AccountType { get; set; }
        public bool HasCompletedOnboarding { get; set; }

        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
    }
} 