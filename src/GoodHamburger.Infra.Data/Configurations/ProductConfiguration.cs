using GoodHamburger.Domain.Entities.Products;
using GoodHamburger.Infra.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infra.Data.Configurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(product => product.Id);
            builder.ConfigureBaseEntity();

            builder.Property(product => product.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(product => product.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(product => product.Description)
                .HasMaxLength(500);

            builder.Property(product => product.Category)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(product => product.IsActive)
                .IsRequired();

            builder.HasData(ProductSeed.Products);
        }
    }
}
