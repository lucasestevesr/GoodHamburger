namespace GoodHamburger.Web.Products.Requests;

public sealed class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public ProductCategory Category { get; set; }

    public bool IsActive { get; set; } = true;
}
