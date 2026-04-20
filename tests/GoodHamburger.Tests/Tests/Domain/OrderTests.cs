using GoodHamburger.Domain.Entities.Base;
using GoodHamburger.Domain.Entities.Orders;
using GoodHamburger.Tests.Unit.Mocks;

namespace GoodHamburger.Tests.Unit.Tests.Domain
{
    public sealed class OrderTests
    {
        #region Testes de desconto

        [Fact]
        public void AddItem_WhenOrderHasBurgerSideAndDrink_AppliesTwentyPercentDiscount()
        {
            var order = CreateOrder();

            order.AddItem(ProductMock.CreateBurger(), 1);
            order.AddItem(ProductMock.CreateSide(), 1);
            order.AddItem(ProductMock.CreateDrink(), 1);

            Assert.Equal(9.50m, order.SubTotal);
            Assert.Equal(0.20m, order.DiscountRate);
            Assert.Equal(7.60m, order.Total);
        }

        [Fact]
        public void AddItem_WhenOrderHasBurgerAndDrink_AppliesFifteenPercentDiscount()
        {
            var order = CreateOrder();

            order.AddItem(ProductMock.CreateBurger(), 1);
            order.AddItem(ProductMock.CreateDrink(), 1);

            Assert.Equal(7.50m, order.SubTotal);
            Assert.Equal(0.15m, order.DiscountRate);
            Assert.Equal(6.3750m, order.Total);
        }

        [Fact]
        public void AddItem_WhenOrderHasBurgerAndSide_AppliesTenPercentDiscount()
        {
            var order = CreateOrder();

            order.AddItem(ProductMock.CreateBurger(), 1);
            order.AddItem(ProductMock.CreateSide(), 1);

            Assert.Equal(7.00m, order.SubTotal);
            Assert.Equal(0.10m, order.DiscountRate);
            Assert.Equal(6.300m, order.Total);
        }

        #endregion

        #region Testes de criacao de itens

        [Fact]
        public void AddItem_WhenProductIsDuplicated_ThrowsDomainException()
        {
            var order = CreateOrder();
            var burger = ProductMock.CreateBurger();

            order.AddItem(burger, 1);

            var exception = Assert.Throws<DomainException>(() => order.AddItem(burger, 1));
            Assert.Contains("duplicados", exception.Message);
        }

        [Fact]
        public void AddItem_WhenCategoryAlreadyExists_ThrowsDomainException()
        {
            var order = CreateOrder();

            order.AddItem(ProductMock.CreateBurger(), 1);

            var exception = Assert.Throws<DomainException>(() => order.AddItem(ProductMock.CreateEggBurger(), 1));
            Assert.Contains("categoria", exception.Message);
        }

        [Fact]
        public void AddItem_WhenProductIsInactive_ThrowsDomainException()
        {
            var order = CreateOrder();
            var burger = ProductMock.CreateBurger();
            burger.IsActive = false;

            var exception = Assert.Throws<DomainException>(() => order.AddItem(burger, 1));
            Assert.Contains("inativo", exception.Message);
        }

        [Fact]
        public void AddItem_WhenQuantityIsInvalid_ThrowsDomainException()
        {
            var order = CreateOrder();

            var exception = Assert.Throws<DomainException>(() => order.AddItem(ProductMock.CreateBurger(), 0));
            Assert.Contains("maior que zero", exception.Message);
        }

        #endregion

        #region Testes de atualizacao e remocao de itens

        [Fact]
        public void UpdateItemQuantity_RecalculatesOrderTotals()
        {
            var order = CreateOrder();
            var burger = ProductMock.CreateBurger();

            order.AddItem(burger, 1);
            order.UpdateItemQuantity(burger.Id, 2);

            Assert.Equal(10.00m, order.SubTotal);
            Assert.Equal(0m, order.DiscountRate);
            Assert.Equal(10.00m, order.Total);
        }

        [Fact]
        public void RemoveItem_RecalculatesOrderTotals()
        {
            var order = CreateOrder();
            var burger = ProductMock.CreateBurger();
            var drink = ProductMock.CreateDrink();

            order.AddItem(burger, 1);
            order.AddItem(drink, 1);
            order.RemoveItem(drink.Id);

            Assert.Single(order.Items);
            Assert.Equal(5.00m, order.SubTotal);
            Assert.Equal(0m, order.DiscountRate);
            Assert.Equal(5.00m, order.Total);
        }

        #endregion

        private static Order CreateOrder()
        {
            return new Order
            {
                CreatedBy = Guid.NewGuid(),
                Status = OrderStatus.Pending
            };
        }
    }
}
