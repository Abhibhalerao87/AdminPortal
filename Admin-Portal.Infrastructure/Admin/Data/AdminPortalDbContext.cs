using Admin_Portal.Core.Admin.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin_Portal.Infrastructure.Admin.Data
{
    public class AdminPortalDbContext : DbContext
    {
        public AdminPortalDbContext(DbContextOptions<AdminPortalDbContext> options) : base(options)
        {
        }

        public DbSet<AdminUser> AdminUsers { get; set; } = null!;

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
            });
        }
    }
}
