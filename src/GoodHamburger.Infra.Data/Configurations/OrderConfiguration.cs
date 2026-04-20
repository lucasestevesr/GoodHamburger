using GoodHamburger.Domain.Entities.Auth;
using GoodHamburger.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infra.Data.Configurations
{
    internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(order => order.Id);
            builder.ConfigureBaseEntity();

            builder.Property(order => order.OrderNumber)
                .IsRequired();

            builder.HasIndex(order => order.OrderNumber)
                .IsUnique();

            builder.Property(order => order.SubTotal)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(order => order.DiscountRate)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(order => order.Total)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(order => order.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(order => order.CreatedBy)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(order => order.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(order => order.Items)
                .WithOne(item => item.Order)
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(order => order.Items)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
