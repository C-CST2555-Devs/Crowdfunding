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
        public DbSet<User> Users { get; set; }
        public DbSet<Pledge> Pledges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Campaign-User relationship
            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.Creator)
                .WithMany(u => u.Campaigns)
                .HasForeignKey(c => c.CreatorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Pledge relationships
            modelBuilder.Entity<Pledge>()
                .HasOne(p => p.Campaign)
                .WithMany()
                .HasForeignKey(p => p.CampaignId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pledge>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pledge>()
                .HasOne(p => p.Reward)
                .WithMany()
                .HasForeignKey(p => p.RewardId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 