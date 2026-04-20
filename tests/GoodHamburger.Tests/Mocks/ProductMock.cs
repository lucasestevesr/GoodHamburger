using GoodHamburger.Domain.Entities.Products;
namespace GoodHamburger.Tests.Unit.Mocks
{
    public static class ProductMock
    {
        public static Product CreateBurger() => new()
        {
            Name = "X Burger",
            Price = 5.00m,
            Category = ProductCategory.Burger,
            IsActive = true
        };
        public static Product CreateEggBurger() => new()
        {
            Name = "X Egg",
            Price = 4.50m,
            Category = ProductCategory.Burger,
            IsActive = true
        };

        public static Product CreateSide() => new()
        {
            Name = "Batata frita",
            Price = 2.00m,
            Category = ProductCategory.Side,
            IsActive = true
        };
        public static Product CreateDrink() => new()
        {
            Name = "Refrigerante",
            Price = 2.50m,
            Category = ProductCategory.Drink,
            IsActive = true
        };
    }
}
