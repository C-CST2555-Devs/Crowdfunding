using Fundify.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Fundify.Web.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FundifyContext(
                serviceProvider.GetRequiredService<DbContextOptions<FundifyContext>>()))
            {
                if (context.Campaigns.Any())
                {
                    return;   // DB has been seeded
                }

                context.Campaigns.AddRange(
                    new Campaign
                    {
                        Title = "CodeAI - Intelligent Code Completion",
                        Category = "AI/ML",
                        Goal = 200000,
                        Description = "Next-gen AI code assistant with real-time collaboration features.",
                        ImageUrl = "/images/ai-project.png",
                        DurationInDays = 15,
                        CreatedAt = DateTime.UtcNow,
                        RaisedAmount = 157500,
                        BackerCount = 324,
                        Rewards = new List<Reward>
                        {
                            new Reward
                            {
                                Name = "Early Bird Access",
                                Amount = 99,
                                Description = "Get early access to CodeAI plus 1 year subscription",
                                EstimatedDelivery = DateTime.UtcNow.AddMonths(3)
                            }
                        }
                    },
                    new Campaign
                    {
                        Title = "DataFlow - Cloud Analytics Suite",
                        Category = "SaaS",
                        Goal = 100000,
                        Description = "Enterprise-grade analytics platform for modern data teams.",
                        ImageUrl = "/images/saas-project.jpg",
                        DurationInDays = 28,
                        CreatedAt = DateTime.UtcNow,
                        RaisedAmount = 45000,
                        BackerCount = 156,
                        Rewards = new List<Reward>
                        {
                            new Reward
                            {
                                Name = "Pro License",
                                Amount = 299,
                                Description = "Lifetime Pro license with priority support",
                                EstimatedDelivery = DateTime.UtcNow.AddMonths(6)
                            }
                        }
                    },
                    new Campaign
                    {
                        Title = "HomeHub - Smart Home Controller",
                        Category = "Hardware",
                        Goal = 100000,
                        Description = "Open-source smart home hub with Matter protocol support.",
                        ImageUrl = "/images/hardware-project.jpg",
                        DurationInDays = 7,
                        CreatedAt = DateTime.UtcNow,
                        RaisedAmount = 90000,
                        BackerCount = 412,
                        Rewards = new List<Reward>
                        {
                            new Reward
                            {
                                Name = "Early Bird Kit",
                                Amount = 199,
                                Description = "HomeHub device with early bird pricing",
                                EstimatedDelivery = DateTime.UtcNow.AddMonths(4)
                            }
                        }
                    }
                );

                context.SaveChanges();
            }
        }
    }
} 