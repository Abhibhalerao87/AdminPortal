using Admin_Portal.Core.Admin.Entities;
using Admin_Portal.Core.Bookings.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin_Portal.Infrastructure.Admin.Data
{
    public class AdminPortalDbContext : DbContext
    {
        public AdminPortalDbContext(DbContextOptions<AdminPortalDbContext> options) : base(options)
        {
        }

        public DbSet<AdminUser> AdminUsers { get; set; } = null!;

        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Configure AdminUser entity
                modelBuilder.Entity<AdminUser>(entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Username)
                        .IsRequired()
                        .HasMaxLength(100);

                    entity.Property(e => e.Email)
                        .IsRequired()
                        .HasMaxLength(256);

                    entity.Property(e => e.PasswordHash)
                        .IsRequired()
                        .HasMaxLength(500);

                    entity.Property(e => e.CreatedAt)
                        .HasDefaultValue(DateTime.UtcNow);

                    // Create unique indexes for Username and Email
                    entity.HasIndex(e => e.Username)
                        .IsUnique();

                    entity.HasIndex(e => e.Email)
                        .IsUnique();

                    // Configure one-to-many relationship with Booking
                    entity.HasMany(e => e.Bookings)
                        .WithOne(b => b.Admin)
                        .HasForeignKey(b => b.AdminId)
                        .OnDelete(DeleteBehavior.SetNull);
                });

                // Configure Booking entity
                modelBuilder.Entity<Booking>(entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.CompanyName)
                        .IsRequired()
                        .HasMaxLength(250);

                    entity.Property(e => e.HrMail)
                        .IsRequired()
                        .HasMaxLength(1000);

                    entity.Property(e => e.Status)
                        .IsRequired()
                        .HasConversion<int>();

                    entity.Property(e => e.RejectionReason)
                        .HasMaxLength(500);

                    entity.Property(e => e.CreatedAt)
                        .HasDefaultValueSql("GETUTCDATE()");

                    entity.Property(e => e.UpdatedAt)
                        .HasDefaultValueSql("GETUTCDATE()");

                    // Create indexes for better query performance
                    entity.HasIndex(e => e.UserId);
                    entity.HasIndex(e => e.AdminId);
                    entity.HasIndex(e => e.Status);
                    entity.HasIndex(e => new { e.UserId, e.Status });
                    entity.HasIndex(e => new { e.AdminId, e.Status });
                });
            }
    }
}
