using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceLicenseSaleApi.Infrastructure.Persistence.Configurations;

public class WeeklyBundleFeatureConfiguration : IEntityTypeConfiguration<WeeklyBundleFeature>
{
    public void Configure(EntityTypeBuilder<WeeklyBundleFeature> builder)
    {
        builder.Property(feature => feature.FeatureKey)
            .HasMaxLength(100)
            .IsRequired();
    }
}