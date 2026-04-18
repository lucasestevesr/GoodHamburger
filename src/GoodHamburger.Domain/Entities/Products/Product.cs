namespace GoodHamburger.Domain.Entities.Products
{
    /// <summary>
    /// Representa um produto na hamburgueria.(Cardápio)
    /// </summary>
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ProductCategory Category { get; set; }
        /// <summary>
        /// Represente se o produto está disponível
        ///</summary>
        public bool IsActive { get; set; }
    }
}