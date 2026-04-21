namespace GoodHamburger.Application.Orders.Requests
{
    /// <summary>
    /// Dados necessários para criar um pedido.
    /// </summary>
    public sealed class CreateOrderRequest
    {
        /// <summary>
        /// Itens iniciais do pedido.
        /// </summary>
        public IReadOnlyCollection<CreateOrderItemRequest>? Items { get; set; }
    }
}
