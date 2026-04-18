using GoodHamburger.Domain.Entities.Base;

namespace GoodHamburger.Domain.Entities.Orders
{
    /// <summary>
    /// Representa uma ordem de compra na hamburgueria.
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Representa o número do pedido.
        /// </summary>
        public long OrderNumber { get; set; }
        
        /// <summary>
        /// Preço antes do desconto.
        /// </summary>
        public decimal SubTotal { get; set; }
       
        /// <summary>
        /// Porcentagem do desconto aplicado.
        /// </summary>
        public decimal Discount { get; set; }
       
        /// <summary>
        /// Identificador do usuário que criou o pedido.
        /// </summary>
        public Guid CreatedBy { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        
    }
}
