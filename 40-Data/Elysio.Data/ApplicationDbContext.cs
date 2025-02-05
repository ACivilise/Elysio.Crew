using Elysio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Elysio.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityByDefaultColumns();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Agent>()
        .HasOne(p => p.Creator)
        .WithMany()
        .HasForeignKey(p => p.CreatorId);

        modelBuilder.Entity<Agent>()
        .HasOne(p => p.Updater)
        .WithMany()
        .HasForeignKey(p => p.UpdaterId);

        modelBuilder.Entity<Room>()
        .HasOne(p => p.Creator)
        .WithMany()
        .HasForeignKey(p => p.CreatorId);

        modelBuilder.Entity<Room>()
        .HasOne(p => p.Updater)
        .WithMany()
        .HasForeignKey(p => p.UpdaterId);
    }
}

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql();

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}