using System.ComponentModel.DataAnnotations;
using GoodHamburger.Domain.Entities.Orders;

namespace GoodHamburger.Application.Orders.Requests
{
    /// <summary>
    /// Dados necessários para atualizar o status de um pedido.
    /// </summary>
    public sealed class UpdateOrderStatusRequest
    {
        /// <summary>
        /// Novo status do pedido.
        /// </summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        public OrderStatus Status { get; set; }
    }
}
