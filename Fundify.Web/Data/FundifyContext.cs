using Microsoft.EntityFrameworkCore;
using Fundify.Web.Models;

namespace Fundify.Web.Data
{
    public class FundifyContext : DbContext
    {
        public FundifyContext(DbContextOptions<FundifyContext> options)
            : base(options)
        {
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Reward> Rewards { get; set; }
    }
} 