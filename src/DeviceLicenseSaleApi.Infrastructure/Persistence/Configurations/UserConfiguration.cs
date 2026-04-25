using DeviceLicenseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceLicenseSaleApi.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
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
        }
    }
}
