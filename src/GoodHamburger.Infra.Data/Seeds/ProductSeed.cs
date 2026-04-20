using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Infra.Data.Seeds
{
    internal static class ProductSeed
    {
        private static readonly DateTimeOffset SeedDate = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        internal static readonly Guid XBurgerId = new("e3ad0326-a72d-4787-a9b4-9a3ab9863366");
        internal static readonly Guid XEggId = new("fdb1f206-ed55-4467-9295-e4a88df7f8bf");
        internal static readonly Guid XBaconId = new("5d109c15-1a8f-4787-a88e-6414218efc88");
        internal static readonly Guid FriesId = new("d8939ca8-d296-4c79-8070-a09fc914f869");
        internal static readonly Guid SodaId = new("44a49d87-daa7-481a-b26a-0b8ee03adc82");

        internal static IEnumerable<Product> Products => new[]
        {
            new Product
            {
                Id = XBurgerId,
                CreationDate = SeedDate,
                Name = "X Burger",
                Price = 5.00m,
                Category = ProductCategory.Burger,
                IsActive = true
            },
            new Product
            {
                Id = XEggId,
                CreationDate = SeedDate,
                Name = "X Egg",
                Price = 4.50m,
                Category = ProductCategory.Burger,
                IsActive = true
            },
            new Product
            {
                Id = XBaconId,
                CreationDate = SeedDate,
                Name = "X Bacon",
                Price = 7.00m,
                Category = ProductCategory.Burger,
                IsActive = true
            },
            new Product
            {
                Id = FriesId,
                CreationDate = SeedDate,
                Name = "Batata frita",
                Price = 2.00m,
                Category = ProductCategory.Side,
                IsActive = true
            },
            new Product
            {
                Id = SodaId,
                CreationDate = SeedDate,
                Name = "Refrigerante",
                Price = 2.50m,
                Category = ProductCategory.Drink,
                IsActive = true
            }
        };
    }
}
