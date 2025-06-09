using Microsoft.EntityFrameworkCore;
using LearningDDD.Domain.Models;

namespace LearningDDD.Infrastructure.Data
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
            modelBuilder.Entity<Group>(builder =>
            {
                builder.HasKey(g => g.Id);
                builder.Property(g => g.Name).IsRequired().HasMaxLength(255);
                builder.Property(g => g.Capacity).IsRequired();

                builder
                .HasMany(g => g.ChargeStations)
                .WithOne()
                .HasForeignKey("GroupId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                builder.Metadata
                .FindNavigation(nameof(Group.ChargeStations))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            });

            modelBuilder.Entity<ChargeStation>(builder => {
                builder.HasKey(cs => cs.Id);
                builder.Property(cs => cs.Name).IsRequired().HasMaxLength(255);

                builder
                .HasMany(cs => cs.Connectors)
                .WithOne()
                .HasForeignKey("ChargeStationId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                builder.Metadata
                .FindNavigation(nameof(ChargeStation.Connectors))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Connector>(builder =>
            {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.MaxCurrent).IsRequired();

                builder
                .HasIndex("ChargeStationContextId", "ChargeStationId")
                .IsUnique();
            });
        }
    }
}