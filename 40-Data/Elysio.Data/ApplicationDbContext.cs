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

    public DbSet<Agent> Agents { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityByDefaultColumns();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Agent>()
       .HasMany(e => e.Rooms)
       .WithMany(e => e.Agents);

        modelBuilder.Entity<Message>()
        .HasOne(p => p.Agent)
        .WithMany()
        .HasForeignKey(p => p.AgentId);

        modelBuilder.Entity<Message>()
        .HasOne(p => p.Conversation)
        .WithMany()
        .HasForeignKey(p => p.ConversationId);

        modelBuilder.Entity<Conversation>()
        .HasOne(p => p.Room)
        .WithMany()
        .HasForeignKey(p => p.RoomId);
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