namespace GoodHamburger.Application.Orders.Responses
{
    /// <summary>
    /// Representa um item retornado no detalhamento do pedido.
    /// </summary>
    public sealed class OrderItemResponse
    {
        /// <summary>
        /// Identificador do produto.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Nome do produto.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Categoria do produto.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Quantidade pedida.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Preço unitário do produto no momento em que foi adicionado ao pedido.
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Valor total da linha do pedido.
        /// </summary>
        public decimal LineTotal { get; set; }
    }
}
