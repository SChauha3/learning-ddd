using Microsoft.EntityFrameworkCore;
using SmartCharging.Api.Models;

namespace SmartCharging.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<ChargeStation> ChargeStations { get; set; }
        public DbSet<Connector> Connectors { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> contextOptions): base(contextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasMany(g => g.ChargeStations)
                .WithOne(cs => cs.Group)
                .HasForeignKey(cs => cs.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChargeStation>()
                .HasMany(cs => cs.Connectors)
                .WithOne(c => c.ChargeStation)
                .HasForeignKey(c => c.ChargeStationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Connector>()
                .HasIndex(c => new { c.ChargeStationContextId, c.ChargeStationId })
                .IsUnique();
        }
    }
}