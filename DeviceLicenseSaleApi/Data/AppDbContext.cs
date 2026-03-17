using Microsoft.EntityFrameworkCore;
using DeviceLicenseSaleApi.Models;

namespace DeviceLicenseSaleApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets map your tables to C# classes
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


        // Later you’ll add more DbSets for other tables
        // e.g. public DbSet<User> Users { get; set; }
        //      public DbSet<License> Licenses { get; set; }
    }
}