namespace GoodHamburger.Web.Orders.Requests;

public sealed class CreateOrderItemRequest
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
