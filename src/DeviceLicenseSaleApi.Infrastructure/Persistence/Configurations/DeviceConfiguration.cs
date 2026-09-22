using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceLicenseSaleApi.Infrastructure.Persistence.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.Property(device => device.SerialNumber)
            .HasMaxLength(64)
            .HasDefaultValue(string.Empty);

        builder.HasIndex(device => device.SerialNumber)
            .IsUnique();
    }
}