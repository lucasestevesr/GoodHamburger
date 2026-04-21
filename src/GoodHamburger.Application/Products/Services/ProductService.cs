using GoodHamburger.Application.Products.Interfaces;
using GoodHamburger.Application.Products.Mappings;
using GoodHamburger.Application.Products.Requests;
using GoodHamburger.Application.Products.Responses;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Application.Products.Services
{
    public sealed class ProductService(IProductRepository products) : IProductService
    {
        public async Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken ct)
        {
            var productList = await products.ListAsync(ct);
            return productList.Select(product => product.ToResponse()).ToList();
        }

        public async Task<ProductResponse> GetByIdAsync(Guid productId, CancellationToken ct)
        {
            EnsureNotEmpty(productId, nameof(productId), "ProductId inválido.");

            var product = await GetProductAsync(productId, ct);
            return product.ToResponse();
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            var product = new Product
            {
                Name = request.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                Category = request.Category,
                IsActive = request.IsActive
            };

            product.PriceValidation(request.Price);

            await products.AddAsync(product, ct);
            await products.SaveChangesAsync(ct);

            return product.ToResponse();
        }

        public async Task<ProductResponse> UpdateAsync(Guid productId, UpdateProductRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);
            EnsureNotEmpty(productId, nameof(productId), "ProductId inválido.");

            var product = await GetProductAsync(productId, ct);

            product.Name = request.Name.Trim();
            product.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
            product.Category = request.Category;
            product.IsActive = request.IsActive;
            product.PriceValidation(request.Price);

            await products.SaveChangesAsync(ct);

            return product.ToResponse();
        }

        public async Task<ProductResponse> UpdateStatusAsync(Guid productId, UpdateProductStatusRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);
            EnsureNotEmpty(productId, nameof(productId), "ProductId inválido.");

            var product = await GetProductAsync(productId, ct);
            product.IsActive = request.IsActive;

            await products.SaveChangesAsync(ct);

            return product.ToResponse();
        }

        public async Task DeleteAsync(Guid productId, CancellationToken ct)
        {
            EnsureNotEmpty(productId, nameof(productId), "ProductId inválido.");

            var product = await GetProductAsync(productId, ct);
            products.Remove(product);

            await products.SaveChangesAsync(ct);
        }

        private async Task<Product> GetProductAsync(Guid productId, CancellationToken ct)
        {
            var product = await products.GetByIdAsync(productId, ct);
            if (product is null)
                throw new KeyNotFoundException("Produto não encontrado.");

            return product;
        }

        private static void EnsureNotEmpty(Guid value, string parameterName, string message)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(message, parameterName);
        }
    }
}
