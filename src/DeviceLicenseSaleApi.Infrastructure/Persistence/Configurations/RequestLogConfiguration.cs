using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceLicenseSaleApi.Infrastructure.Persistence.Configurations
{
    public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
    {
        public void Configure(EntityTypeBuilder<RequestLog> entity)
        {
            entity.ToTable("RequestLogs");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Method)
                .HasMaxLength(16)
                .IsRequired();

            entity.Property(x => x.Path)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.QueryString)
                .HasMaxLength(1000);

            entity.Property(x => x.RemoteIpAddress)
                .HasMaxLength(120);

            entity.Property(x => x.Username)
                .HasMaxLength(120);

            entity.Property(x => x.TraceId)
                .HasMaxLength(120);

            entity.HasIndex(x => x.TimestampUtc);
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.StatusCode);
        }
    }
}
