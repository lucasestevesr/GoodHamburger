namespace GoodHamburger.Application.Orders.Responses
{
    /// <summary>
    /// Representa o pedido completo retornado pela API.
    /// </summary>
    public sealed class OrderResponse
    {
        /// <summary>
        /// Identificador do pedido.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Número sequencial do pedido.
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// Identificador do usuário que criou o pedido.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Status atual do pedido.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Soma dos itens antes do desconto.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Percentual de desconto aplicado ao pedido.
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// Valor final do pedido após o desconto.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Data de criação do pedido.
        /// </summary>
        public DateTimeOffset CreationDate { get; set; }

        /// <summary>
        /// Itens do pedido.
        /// </summary>
        public IReadOnlyList<OrderItemResponse> Items { get; set; } = [];
    }
}
