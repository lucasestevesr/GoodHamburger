using GoodHamburger.Web.Infrastructure.Http;
using GoodHamburger.Web.Products.Requests;
using GoodHamburger.Web.Products.Responses;

namespace GoodHamburger.Web.Products;

public sealed class ProductsApiClient(ApiHttpClient api)
{
    public Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken ct = default)
    {
        return api.GetAsync<IReadOnlyList<ProductResponse>>("api/v1/products", ct);
    }

    public Task<ProductResponse> GetByIdAsync(Guid productId, CancellationToken ct = default)
    {
        return api.GetAsync<ProductResponse>($"api/v1/products/{productId}", ct);
    }

    public Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        return api.PostAsync<ProductResponse>("api/v1/products", request, ct);
    }

    public Task<ProductResponse> UpdateAsync(Guid productId, UpdateProductRequest request, CancellationToken ct = default)
    {
        return api.PutAsync<ProductResponse>($"api/v1/products/{productId}", request, ct);
    }

    public Task<ProductResponse> UpdateStatusAsync(Guid productId, bool isActive, CancellationToken ct = default)
    {
        return api.PatchAsync<ProductResponse>(
            $"api/v1/products/{productId}/status",
            new UpdateProductStatusRequest { IsActive = isActive },
            ct);
    }

    public Task DeleteAsync(Guid productId, CancellationToken ct = default)
    {
        return api.DeleteAsync($"api/v1/products/{productId}", ct);
    }
}
