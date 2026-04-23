namespace GoodHamburger.Web.Orders.Requests;

public sealed class AddOrderItemRequest
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
