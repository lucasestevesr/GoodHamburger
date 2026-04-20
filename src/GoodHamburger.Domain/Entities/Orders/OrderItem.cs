using GoodHamburger.Domain.Entities.Base;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Domain.Entities.Orders
{
    public sealed class OrderItem
    {
        public Order Order { get; set; } = null!;

        public Guid OrderId { get; set; }

        public Product Product { get; set; } = null!;

        public Guid ProductId { get; set; }

        public int Quantity { get; private set; }

        public decimal ProductPrice { get; set; }

        public ProductCategory Category { get; set; }

        public decimal LineTotal => ProductPrice * Quantity;

        public void ValidateQuantity(int quantity, Product product)
        {
            if (quantity <= 0)
                throw new DomainException($"Quantidade do produto '{product.Name}' deve ser maior que zero.");

            Quantity = quantity;
        }
    }
}
