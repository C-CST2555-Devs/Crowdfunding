using Fundify.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Fundify.Web.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new FundifyContext(
                serviceProvider.GetRequiredService<DbContextOptions<FundifyContext>>());

            // Return if DB is not empty
            if (context.Campaigns.Any())
            {
                return;
            }

            // First create a default admin user
            var adminUser = new User
            {
                Email = "admin@fundify.com",
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                AccountType = AccountType.Administrator,
                HasCompletedOnboarding = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
} 