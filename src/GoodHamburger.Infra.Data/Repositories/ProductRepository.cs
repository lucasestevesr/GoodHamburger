using GoodHamburger.Application.Products.Interfaces;
using GoodHamburger.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infra.Data.Repositories
{
    public sealed class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await context.Products
                .SingleOrDefaultAsync(product => product.Id == id, ct);
        }

        public async Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct)
        {
            return await context.Products
                .AsNoTracking()
                .OrderBy(product => product.Category)
                    .ThenBy(product => product.Name)
                .ToListAsync(ct);
        }

        public async Task AddAsync(Product product, CancellationToken ct)
        {
            await context.Products.AddAsync(product, ct);
        }

        public void Remove(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await context.SaveChangesAsync(ct);
        }
    }
}
