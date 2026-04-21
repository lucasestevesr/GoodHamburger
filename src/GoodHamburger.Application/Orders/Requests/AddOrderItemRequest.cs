using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Application.Orders.Requests
{
    /// <summary>
    /// Dados necessários para adicionar um item ao pedido.
    /// </summary>
    public sealed class AddOrderItemRequest
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
