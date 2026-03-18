using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Administrative> Administratives { get; set; }
        public DbSet<CallAnswering> CallAnswerings { get; set; }
        public DbSet<CallManagement> CallManagements { get; set; }
        public DbSet<CallScreening> CallScreenings { get; set; }
        public DbSet<CostBandwidthSaving> CostBandwidthSavings { get; set; }
        public DbSet<GroupConvenience> GroupConveniences { get; set; }
        public DbSet<SecurityTools> SecurityTools { get; set; }
        public DbSet<UnifiedCommunications> UnifiedCommunications { get; set; }
        public DbSet<Utility> Utilities { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<LicensableFeatures> LicensableFeatures { get; set; }
        public DbSet<DeviceTypes> DeviceTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<License> Licenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasIndex(x => x.Username).IsUnique();

                entity.Property(x => x.Username)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.Email)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(x => x.PasswordHash)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Role)
                    .HasMaxLength(20)
                    .HasDefaultValue("User")
                    .IsRequired();

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasIndex(x => x.UserId).IsUnique();

                entity.Property(x => x.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Phone)
                    .HasMaxLength(30);

                entity.Property(x => x.Address)
                    .HasMaxLength(300);

                entity.HasOne(x => x.User)
                    .WithOne()
                    .HasForeignKey<UserProfile>(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
