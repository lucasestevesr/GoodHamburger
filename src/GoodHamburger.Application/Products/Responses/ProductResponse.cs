namespace GoodHamburger.Application.Products.Responses
{
    public sealed class ProductResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string Category { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTimeOffset CreationDate { get; set; }
    }
}
