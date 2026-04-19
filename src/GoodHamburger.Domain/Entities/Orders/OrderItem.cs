using GoodHamburger.Domain.Entities.Base;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Domain.Entities.Orders
{
    public sealed class OrderItem
    {
        public Order Order { get; set; } = null!;
        public Guid OrderId { get; set; }
       
        public Product Product { get; set; } =  null!;
        public Guid ProductId { get; set; }

        public int Quantity { get; private set; }
        
        /// <summary>
        /// Representa o preço do produto(unitário) no momento do pedido.
        /// </summary>
        public decimal ProductPrice { get; set; }
       
        /// <summary>
        /// Representa a categoria do produto no momento do pedido.
        /// </summary>
        public ProductCategory Category { get; set; }
       
        public decimal LineTotal => ProductPrice * Quantity;

        public void QuantityValidation(int quantity, Product product)
        {
            if (quantity <= 0)
                throw new DomainException($"Quantidade do produto '{product.Name}' deve ser maior que zero.");

            Quantity = quantity;
        }
    }
}
