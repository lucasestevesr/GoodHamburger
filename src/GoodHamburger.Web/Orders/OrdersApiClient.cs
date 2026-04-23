using GoodHamburger.Web.Infrastructure.Http;
using GoodHamburger.Web.Orders.Requests;
using GoodHamburger.Web.Orders.Responses;

namespace GoodHamburger.Web.Orders;

public sealed class OrdersApiClient(ApiHttpClient api)
{
    public Task<IReadOnlyList<OrderSummaryResponse>> ListAsync(CancellationToken ct = default)
    {
        return api.GetAsync<IReadOnlyList<OrderSummaryResponse>>("api/v1/orders", ct);
    }

    public Task<OrderResponse> GetByIdAsync(Guid orderId, CancellationToken ct = default)
    {
        return api.GetAsync<OrderResponse>($"api/v1/orders/{orderId}", ct);
    }

    public Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        return api.PostAsync<OrderResponse>("api/v1/orders", request, ct);
    }

    public Task<OrderResponse> AddItemAsync(Guid orderId, AddOrderItemRequest request, CancellationToken ct = default)
    {
        return api.PostAsync<OrderResponse>($"api/v1/orders/{orderId}/items", request, ct);
    }

    public Task<OrderResponse> UpdateItemQuantityAsync(Guid orderId, Guid productId, int quantity, CancellationToken ct = default)
    {
        return api.PutAsync<OrderResponse>(
            $"api/v1/orders/{orderId}/items/{productId}",
            new UpdateOrderItemQuantityRequest { Quantity = quantity },
            ct);
    }

    public Task RemoveItemAsync(Guid orderId, Guid productId, CancellationToken ct = default)
    {
        return api.DeleteAsync($"api/v1/orders/{orderId}/items/{productId}", ct);
    }

    public Task<OrderResponse> UpdateStatusAsync(Guid orderId, OrderStatus status, CancellationToken ct = default)
    {
        return api.PatchAsync<OrderResponse>(
            $"api/v1/orders/{orderId}/status",
            new UpdateOrderStatusRequest { Status = status },
            ct);
    }

    public Task DeleteAsync(Guid orderId, CancellationToken ct = default)
    {
        return api.DeleteAsync($"api/v1/orders/{orderId}", ct);
    }
}
