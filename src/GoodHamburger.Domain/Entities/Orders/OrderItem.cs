using GoodHamburger.Domain.Entities.Base;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity
    {
        public int Quantity { get; set; }       
        /// <summary>
        /// Representa o preço do produto(unitário) no momento do pedido.
        /// </summary>
        public decimal ProductPrice { get; set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
    }
}
