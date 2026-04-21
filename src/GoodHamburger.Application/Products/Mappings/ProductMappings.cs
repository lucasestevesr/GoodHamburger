using GoodHamburger.Application.Products.Responses;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Application.Products.Mappings
{
    internal static class ProductMappings
    {
        internal static ProductResponse ToResponse(this Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category.ToString(),
                IsActive = product.IsActive,
                CreationDate = product.CreationDate
            };
        }
    }
}
