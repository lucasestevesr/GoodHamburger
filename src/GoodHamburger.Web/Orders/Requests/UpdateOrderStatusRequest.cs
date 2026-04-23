namespace GoodHamburger.Web.Orders.Requests;

public sealed class UpdateOrderStatusRequest
{
    public OrderStatus Status { get; set; }
}
