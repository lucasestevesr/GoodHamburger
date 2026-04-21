using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Application.Orders.Requests
{
    /// <summary>
    /// Dados necessários para atualizar a quantidade de um item do pedido.
    /// </summary>
    public sealed class UpdateOrderItemQuantityRequest
    {
        /// <summary>
        /// Nova quantidade do item.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantity { get; set; }
    }
}
