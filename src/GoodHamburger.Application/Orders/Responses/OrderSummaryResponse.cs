namespace GoodHamburger.Application.Orders.Responses
{
    public sealed class OrderSummaryResponse
    {
        public Guid Id { get; set; }

        public long OrderNumber { get; set; }

        public Guid CreatedBy { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal Total { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public int ItemCount { get; set; }
    }
}
