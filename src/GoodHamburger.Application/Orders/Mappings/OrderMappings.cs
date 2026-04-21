using GoodHamburger.Application.Orders.Responses;
using GoodHamburger.Domain.Entities.Orders;

namespace GoodHamburger.Application.Orders.Mappings
{
    internal static class OrderMappings
    {
        internal static OrderResponse ToResponse(this Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedBy = order.CreatedBy,
                Status = order.Status.ToString(),
                SubTotal = order.SubTotal,
                DiscountRate = order.DiscountRate,
                Total = order.Total,
                CreationDate = order.CreationDate,
                Items = order.Items
                    .Select(item => new OrderItemResponse
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product?.Name ?? string.Empty,
                        Category = item.Category.ToString(),
                        Quantity = item.Quantity,
                        ProductPrice = item.ProductPrice,
                        LineTotal = item.LineTotal
                    })
                    .ToList()
            };
        }

        internal static OrderSummaryResponse ToSummaryResponse(this Order order)
        {
            return new OrderSummaryResponse
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedBy = order.CreatedBy,
                Status = order.Status.ToString(),
                SubTotal = order.SubTotal,
                DiscountRate = order.DiscountRate,
                Total = order.Total,
                CreationDate = order.CreationDate,
                ItemCount = order.Items.Count
            };
        }
    }
}
