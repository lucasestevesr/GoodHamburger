using System.ComponentModel.DataAnnotations;
using System.Globalization;
using GoodHamburger.Application.Products.Requests;
using GoodHamburger.Domain.Entities.Products;

namespace GoodHamburger.Tests.Unit.Tests.Application
{
    public sealed class ProductRequestValidationTests
    {
        [Fact]
        public void CreateProductRequest_WhenCultureUsesCommaDecimalSeparator_AcceptsJsonDecimalValue()
        {
            var request = new CreateProductRequest
            {
                Name = "X Burger",
                Price = 8.75m,
                Category = ProductCategory.Burger,
                IsActive = true
            };

            var validationResults = ValidateWithCulture(request, "pt-BR");

            Assert.Empty(validationResults);
        }

        [Fact]
        public void UpdateProductRequest_WhenCultureUsesCommaDecimalSeparator_AcceptsJsonDecimalValue()
        {
            var request = new UpdateProductRequest
            {
                Name = "X Burger",
                Price = 8.75m,
                Category = ProductCategory.Burger,
                IsActive = true
            };

            var validationResults = ValidateWithCulture(request, "pt-BR");

            Assert.Empty(validationResults);
        }

        private static IReadOnlyCollection<ValidationResult> ValidateWithCulture(object request, string cultureName)
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                var culture = CultureInfo.GetCultureInfo(cultureName);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(
                    request,
                    new ValidationContext(request),
                    validationResults,
                    validateAllProperties: true);

                return validationResults;
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }
    }
}
