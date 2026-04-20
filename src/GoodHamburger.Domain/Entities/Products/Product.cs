using GoodHamburger.Domain.Entities.Base;

namespace GoodHamburger.Domain.Entities.Products
{
    public sealed class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public ProductCategory Category { get; set; }

        public bool IsActive { get; set; }

        public void PriceValidation(decimal price)
        {
            if (price <= 0)
                throw new DomainException("Preço deve ser maior que zero.");

            Price = price;
        }
    }
}
