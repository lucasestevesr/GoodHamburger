using GoodHamburger.Domain.Entities.Auth;
using GoodHamburger.Infra.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infra.Data.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(user => user.Id);
            builder.ConfigureBaseEntity();

            builder.Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.Property(user => user.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(user => user.Role)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(user => user.IsActive)
                .IsRequired();

            builder.HasData(AuthSeed.Users);
        }
    }
}
