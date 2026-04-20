using GoodHamburger.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infra.Data.Configurations
{
    internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(item => new { item.OrderId, item.ProductId });

            builder.Property(item => item.Quantity)
                .IsRequired();

            builder.Property(item => item.ProductPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(item => item.Category)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(item => item.Product)
                .WithMany()
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
