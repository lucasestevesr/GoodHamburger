namespace GoodHamburger.Application.Orders.Requests
{
    /// <summary>
    /// Dados necessários para criar um pedido.
    /// </summary>
    public sealed class CreateOrderRequest
    {
        /// <summary>
        /// Identificador do usuário responsável pela criação do pedido.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Itens iniciais do pedido.
        /// </summary>
        public IReadOnlyCollection<CreateOrderItemRequest>? Items { get; set; }
    }
}
