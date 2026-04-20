using GoodHamburger.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infra.Data.Configurations
{
    internal static class BaseEntityConfiguration
    {
        internal static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : BaseEntity
        {
            builder.Property(entity => entity.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            builder.Property(entity => entity.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')")
                .IsRequired();
        }
    }
}
