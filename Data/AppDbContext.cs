using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Campaign> Campaigns { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
} 