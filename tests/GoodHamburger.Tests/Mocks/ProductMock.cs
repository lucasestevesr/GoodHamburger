using GoodHamburger.Domain.Entities.Products;
namespace GoodHamburger.Tests.Unit.Mocks
{
    public static class ProductMock
    {
        public static Product CreateBurger() => new()
        {
            Id = new Guid("e3ad0326-a72d-4787-a9b4-9a3ab9863366"),
            Name = "X Burger",
            Price = 5.00m,
            Category = ProductCategory.Burger,
            IsActive = true
        };
        public static Product CreateEggBurger() => new()
        {
            Id = new Guid("fdb1f206-ed55-4467-9295-e4a88df7f8bf"),
            Name = "X Egg",
            Price = 4.50m,
            Category = ProductCategory.Burger,
            IsActive = true
        };

        public static Product CreateSide() => new()
        {
            Id = new Guid("d8939ca8-d296-4c79-8070-a09fc914f869"),
            Name = "Batata frita",
            Price = 2.00m,
            Category = ProductCategory.Side,
            IsActive = true
        };
        public static Product CreateDrink() => new()
        {
            Id = new Guid("44a49d87-daa7-481a-b26a-0b8ee03adc82"),
            Name = "Refrigerante",
            Price = 2.50m,
            Category = ProductCategory.Drink,
            IsActive = true
        };
    }
}
