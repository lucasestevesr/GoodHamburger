using System.ComponentModel.DataAnnotations;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Application.Products.Requests
{
    public sealed class CreateProductRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Range(
            typeof(decimal),
            "0.01",
            "999999999",
            ErrorMessage = "O preço deve ser maior que zero.",
            ParseLimitsInInvariantCulture = true,
            ConvertValueInInvariantCulture = true)]
        public decimal Price { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public ProductCategory Category { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
