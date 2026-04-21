using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Application.Orders.Requests
{
    /// <summary>
    /// Item enviado na criação de um pedido.
    /// </summary>
    public sealed class CreateOrderItemRequest
    {
        /// <summary>
        /// Identificador do produto.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Quantidade solicitada do produto.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantity { get; set; }
    }
}
