using GoodHamburger.Application.Orders.Interfaces;
using GoodHamburger.Application.Orders.Mappings;
using GoodHamburger.Application.Orders.Requests;
using GoodHamburger.Application.Orders.Responses;
using GoodHamburger.Application.Products.Interfaces;
using GoodHamburger.Application.Users.Interfaces;
using GoodHamburger.Domain.Entities.Orders;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Application.Orders.Services
{
    public sealed class OrderService(
        IOrderRepository orders,
        IProductRepository products) : IOrderService
    {
        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken ct)
        {
            var order = new Order
            {
                CreatedBy = request.CreatedBy,
                Status = OrderStatus.Pending,
                OrderNumber = await orders.GetNextOrderNumberAsync(ct)
            };

            if (request.Items is null || request.Items.Count == 0)
            {
                order.CalculateTotalPrice();
            }
            else
            {
                foreach (var item in request.Items)
                {
                    var product = await GetProductAsync(item.ProductId, ct);
                    order.AddItem(product, item.Quantity);
                }
            }

            await orders.AddAsync(order, ct);
            await orders.SaveChangesAsync(ct);

            return order.ToResponse();
        }

        public async Task<IReadOnlyList<OrderSummaryResponse>> ListAsync(CancellationToken ct)
        {
            var orderList = await orders.ListAsync(ct);

            return orderList
                .OrderByDescending(order => order.OrderNumber)
                .Select(order => order.ToSummaryResponse())
                .ToList();
        }

        public async Task<OrderResponse> GetByIdAsync(Guid orderId, CancellationToken ct)
        {
            var order = await GetOrderAsync(orderId, ct);
            return order.ToResponse();
        }

        public async Task<OrderResponse> AddItemAsync(Guid orderId, AddOrderItemRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            var order = await GetOrderAsync(orderId, ct);
            var product = await GetProductAsync(request.ProductId, ct);

            order.AddItem(product, request.Quantity);

            await orders.SaveChangesAsync(ct);

            return order.ToResponse();
        }

        public async Task<OrderResponse> UpdateItemQuantityAsync(
            Guid orderId,
            Guid productId,
            UpdateOrderItemQuantityRequest request,
            CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            var order = await GetOrderAsync(orderId, ct);

            order.UpdateItemQuantity(productId, request.Quantity);

            await orders.SaveChangesAsync(ct);

            return order.ToResponse();
        }

        public async Task RemoveItemAsync(Guid orderId, Guid productId, CancellationToken ct)
        {
            var order = await GetOrderAsync(orderId, ct);

            order.RemoveItem(productId);

            await orders.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid orderId, CancellationToken ct)
        {
            var order = await GetOrderAsync(orderId, ct);

            orders.Remove(order);
            await orders.SaveChangesAsync(ct);
        }

        private async Task<Order> GetOrderAsync(Guid orderId, CancellationToken ct)
        {
            var order = await orders.GetByIdAsync(orderId, ct);
            if (order is null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            return order;
        }

        private async Task<Product> GetProductAsync(Guid productId, CancellationToken ct)
        {
            var product = await products.GetByIdAsync(productId, ct);
            if (product is null)
                throw new KeyNotFoundException("Produto não encontrado.");

            return product;
        }
    }
}
