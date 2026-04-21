namespace GoodHamburger.Application.Orders.Responses
{
    public sealed class OrderItemResponse
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal LineTotal { get; set; }
    }
}
