using Microsoft.EntityFrameworkCore;
using TripBookingApi.Domain;

namespace TripBookingApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Trip> Trips { get; set; }
    public DbSet<Registration> Registrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedNever();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.HasIndex(e => e.Name)
                .IsUnique();
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Description);
            entity.Property(e => e.StartDate)
                .IsRequired();
            entity.Property(e => e.NumberOfSeats)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasMany(e => e.Registrations)
                .WithOne(r => r.Trip)
                .HasForeignKey(r => r.TripId);
            //TODO: Not working for in-memory database
            // entity.Property(e => e.RowVersion)
            //     .IsRowVersion();
            // entity.Property(a => a.RowVersion)
            //     .IsConcurrencyToken()
            //     .ValueGeneratedOnAddOrUpdate();
        });
        
        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedNever();
            entity.Property(e => e.Email)
                .IsRequired();
            entity.HasIndex(e => new { e.Email, e.TripId })
                .IsUnique();
        });
    }
}