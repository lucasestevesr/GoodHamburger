namespace GoodHamburger.Web.Orders.Requests;

public sealed class CreateOrderRequest
{
    public IReadOnlyCollection<CreateOrderItemRequest>? Items { get; set; }
}
