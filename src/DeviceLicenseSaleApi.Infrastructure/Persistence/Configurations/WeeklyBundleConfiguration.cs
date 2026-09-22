using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceLicenseSaleApi.Infrastructure.Persistence.Configurations;

public class WeeklyBundleConfiguration : IEntityTypeConfiguration<WeeklyBundle>
{
    public void Configure(EntityTypeBuilder<WeeklyBundle> builder)
    {
        builder.Property(bundle => bundle.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(bundle => bundle.DiscountPercentage)
            .HasColumnType("decimal(5,2)");

        builder.HasMany(bundle => bundle.Features)
            .WithOne(feature => feature.WeeklyBundle)
            .HasForeignKey(feature => feature.WeeklyBundleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}