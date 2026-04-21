using GoodHamburger.Api.Security;
using GoodHamburger.Application.Orders.Interfaces;
using GoodHamburger.Application.Orders.Requests;
using GoodHamburger.Application.Orders.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers
{
    /// <summary>
    /// Endpoints para gerenciamento de pedidos.
    /// </summary>
    [ApiController]
    [Authorize(Policy = AuthorizationPolicies.OrderManagement)]
    [Route("api/v1/orders")]
    public sealed class OrdersController(IOrderService orderService) : ControllerBase
    {
        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
        {
            var createdOrder = await orderService.CreateAsync(request, ct);

            return CreatedAtAction(nameof(GetById), new { orderId = createdOrder.Id }, createdOrder);
        }

        /// <summary>
        /// Lista os pedidos cadastrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<OrderSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(CancellationToken ct)
        {
            var orderList = await orderService.ListAsync(ct);
            return Ok(orderList);
        }

        /// <summary>
        /// Obtém um pedido pelo identificador.
        /// </summary>
        [HttpGet("{orderId:guid}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid orderId, CancellationToken ct)
        {
            var order = await orderService.GetByIdAsync(orderId, ct);
            return Ok(order);
        }

        /// <summary>
        /// Adiciona um item a um pedido existente.
        /// </summary>
        [HttpPost("{orderId:guid}/items")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItem(Guid orderId, [FromBody] AddOrderItemRequest request, CancellationToken ct)
        {
            var updatedOrder = await orderService.AddItemAsync(orderId, request, ct);
            return Ok(updatedOrder);
        }
        
        /// <summary>
        /// Atualiza o status de um pedido.
        /// </summary>
        [HttpPatch("{orderId:guid}/status")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(Guid orderId, [FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
        {
            var updatedOrder = await orderService.UpdateStatusAsync(orderId, request, ct);
            return Ok(updatedOrder);
        }

        /// <summary>
        /// Atualiza a quantidade de um item do pedido.
        /// </summary>
        [HttpPut("{orderId:guid}/items/{productId:guid}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItemQuantity(
            Guid orderId,
            Guid productId,
            [FromBody] UpdateOrderItemQuantityRequest request,
            CancellationToken ct)
        {
            var updatedOrder = await orderService.UpdateItemQuantityAsync(orderId, productId, request, ct);
            return Ok(updatedOrder);
        }

        /// <summary>
        /// Remove um item de um pedido.
        /// </summary>
        [HttpDelete("{orderId:guid}/items/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItem(Guid orderId, Guid productId, CancellationToken ct)
        {
            await orderService.RemoveItemAsync(orderId, productId, ct);
            return NoContent();
        }

        /// <summary>
        /// Remove um pedido pelo identificador.
        /// </summary>
        [HttpDelete("{orderId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid orderId, CancellationToken ct)
        {
            await orderService.DeleteAsync(orderId, ct);
            return NoContent();
        }
    }
}
