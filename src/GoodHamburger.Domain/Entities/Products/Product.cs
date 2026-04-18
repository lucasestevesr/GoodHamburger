using GoodHamburger.Domain.Entities.Base;

namespace GoodHamburger.Domain.Entities.Products
{
    /// <summary>
    /// Representa um produto na hamburgueria.(Cardápio)
    /// </summary>
    public sealed class Product : BaseEntity
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public ProductCategory Category { get; set; }

        /// <summary>
        /// Represente se o produto está disponível
        ///</summary>
        public bool IsActive { get; set; }
    }
}