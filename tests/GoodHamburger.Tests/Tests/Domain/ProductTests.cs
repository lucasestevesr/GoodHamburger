using GoodHamburger.Domain.Entities.Base;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Tests.Unit.Tests.Domain
{
    public sealed class ProductTests
    {
        [Fact]
        public void PriceValidation_WhenPriceIsPositive_UpdatesPrice()
        {
            var product = CreateProduct();

            product.PriceValidation(12.50m);

            Assert.Equal(12.50m, product.Price);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void PriceValidation_WhenPriceIsInvalid_ThrowsDomainException(decimal price)
        {
            var product = CreateProduct();

            var exception = Assert.Throws<DomainException>(() => product.PriceValidation(price));

            Assert.Contains("maior que zero", exception.Message);
        }

        [Fact]
        public void Product_ExposesConfiguredState()
        {
            var id = Guid.NewGuid();
            var creationDate = DateTimeOffset.UtcNow;

            var product = new Product
            {
                Id = id,
                CreationDate = creationDate,
                Name = "X Tudo",
                Price = 15.75m,
                Description = "Burger completo",
                Category = ProductCategory.Burger,
                IsActive = true
            };

            Assert.Equal(id, product.Id);
            Assert.Equal(creationDate, product.CreationDate);
            Assert.Equal("X Tudo", product.Name);
            Assert.Equal(15.75m, product.Price);
            Assert.Equal("Burger completo", product.Description);
            Assert.Equal(ProductCategory.Burger, product.Category);
            Assert.True(product.IsActive);
        }

        private static Product CreateProduct()
        {
            return new Product
            {
                Name = "X Burger",
                Category = ProductCategory.Burger,
                IsActive = true
            };
        }
    }
}
