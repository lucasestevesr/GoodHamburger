using GoodHamburger.Application.Orders.Requests;
using GoodHamburger.Application.Orders.Responses;

namespace GoodHamburger.Application.Orders.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken ct);
        Task<IReadOnlyList<OrderSummaryResponse>> ListAsync(CancellationToken ct);
        Task<OrderResponse> GetByIdAsync(Guid orderId, CancellationToken ct);
        Task<OrderResponse> AddItemAsync(Guid orderId, AddOrderItemRequest request, CancellationToken ct);
        Task<OrderResponse> UpdateItemQuantityAsync(Guid orderId, Guid productId, UpdateOrderItemQuantityRequest request, CancellationToken ct);
        Task RemoveItemAsync(Guid orderId, Guid productId, CancellationToken ct);
        Task DeleteAsync(Guid orderId, CancellationToken ct);
    }
}
