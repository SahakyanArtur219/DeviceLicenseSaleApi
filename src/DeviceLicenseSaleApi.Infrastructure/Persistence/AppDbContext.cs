using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Infrastructure.Persistence.Configurations;
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
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<WeeklyBundle> WeeklyBundles { get; set; }
        public DbSet<WeeklyBundleFeature> WeeklyBundleFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ActivityLogConfiguration());
            modelBuilder.ApplyConfiguration(new RequestLogConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new WeeklyBundleConfiguration());
            modelBuilder.ApplyConfiguration(new WeeklyBundleFeatureConfiguration());
        }
    }
}