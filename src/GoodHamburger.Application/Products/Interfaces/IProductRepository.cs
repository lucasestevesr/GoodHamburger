using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Application.Products.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct);
        Task AddAsync(Product product, CancellationToken ct);
        void Remove(Product product);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
