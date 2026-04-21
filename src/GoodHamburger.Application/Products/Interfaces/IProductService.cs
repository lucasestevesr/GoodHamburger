using GoodHamburger.Application.Products.Requests;
using GoodHamburger.Application.Products.Responses;

namespace GoodHamburger.Application.Products.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken ct);
        Task<ProductResponse> GetByIdAsync(Guid productId, CancellationToken ct);
        Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken ct);
        Task<ProductResponse> UpdateAsync(Guid productId, UpdateProductRequest request, CancellationToken ct);
        Task<ProductResponse> UpdateStatusAsync(Guid productId, UpdateProductStatusRequest request, CancellationToken ct);
        Task DeleteAsync(Guid productId, CancellationToken ct);
    }
}
