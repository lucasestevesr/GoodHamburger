using GoodHamburger.Domain.Entities.Base;
using GoodHamburger.Domain.Entities.Orders;
using GoodHamburger.Domain.Entities.Products;
using GoodHamburger.Tests.Unit.Mocks;

namespace GoodHamburger.Tests.Unit.Tests.Domain
{
    public sealed class OrderItemTests
    {
        [Fact]
        public void ValidateQuantity_WhenQuantityIsPositive_UpdatesQuantityAndLineTotal()
        {
            var product = ProductMock.CreateBurger();
            var item = CreateOrderItem(product);

            item.ValidateQuantity(3, product);

            Assert.Equal(3, item.Quantity);
            Assert.Equal(15.00m, item.LineTotal);
        }

        [Fact]
        public void ValidateQuantity_WhenQuantityIsInvalid_ThrowsDomainException()
        {
            var product = ProductMock.CreateBurger();
            var item = CreateOrderItem(product);

            var exception = Assert.Throws<DomainException>(() => item.ValidateQuantity(0, product));

            Assert.Contains("maior que zero", exception.Message);
        }

        [Fact]
        public void OrderItem_ExposesConfiguredState()
        {
            var order = new Order();
            var product = ProductMock.CreateDrink();
            var orderId = Guid.NewGuid();

            var item = new OrderItem
            {
                Order = order,
                OrderId = orderId,
                Product = product,
                ProductId = product.Id,
                ProductPrice = product.Price,
                Category = product.Category
            };

            Assert.Same(order, item.Order);
            Assert.Equal(orderId, item.OrderId);
            Assert.Same(product, item.Product);
            Assert.Equal(product.Id, item.ProductId);
            Assert.Equal(product.Price, item.ProductPrice);
            Assert.Equal(ProductCategory.Drink, item.Category);
        }

        private static OrderItem CreateOrderItem(Product product)
        {
            return new OrderItem
            {
                Order = new Order(),
                Product = product,
                ProductId = product.Id,
                ProductPrice = product.Price,
                Category = product.Category
            };
        }
    }
}
