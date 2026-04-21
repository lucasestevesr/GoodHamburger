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
    }
}
