using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GoodHamburger.Infra.Identity
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // By default, Identity tables are created in the default schema (usually "dbo").
            // So, we need to specify that they should be created in the "auth" schema.
            // This loop iterates through all entity types in the model and checks if their table name starts with "AspNet",
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.GetTableName()?.StartsWith("AspNet") == true)
                {
                    entity.SetSchema("Auth");
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}


