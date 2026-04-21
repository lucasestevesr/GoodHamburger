using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Application.Products.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Product>> ListAsync(CancellationToken ct);
    }
}
