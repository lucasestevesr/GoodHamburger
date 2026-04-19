using GoodHamburger.Domain.Entities.Base;

namespace GoodHamburger.Domain.Entities.Products
{
    /// <summary>
    /// Representa um produto na hamburgueria.(Cardápio)
    /// </summary>
    public sealed class Product : BaseEntity
    {
        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public string? Description { get; private set; }

        public ProductCategory Category { get; private set; }
        /// <summary>
        /// Represente se o produto está disponível
        ///</summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Validação do preço do produto que deve ser maior que zero.
        /// </summary>
        /// <param name="price"></param>
        /// <exception cref="DomainException"></exception>
        public void PriceValidation(decimal price)
        {
            if (price <= 0)
                throw new DomainException("Preço deve ser maior que zero.");

            Price = price;
        }
    }
}