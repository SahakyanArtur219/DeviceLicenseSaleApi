using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceLicenseSaleApi.Infrastructure.Persistence.Configurations
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> entity)
        {
            entity.ToTable("ActivityLogs");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Category)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(x => x.Action)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Outcome)
                .HasMaxLength(40)
                .IsRequired();

            entity.Property(x => x.Username)
                .HasMaxLength(120);

            entity.Property(x => x.EntityName)
                .HasMaxLength(120);

            entity.Property(x => x.EntityId)
                .HasMaxLength(120);

            entity.Property(x => x.TraceId)
                .HasMaxLength(120);

            entity.Property(x => x.DetailsJson)
                .HasColumnType("nvarchar(max)");

            entity.HasIndex(x => x.TimestampUtc);
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.Category);
            entity.HasIndex(x => x.Outcome);
        }
    }
}
