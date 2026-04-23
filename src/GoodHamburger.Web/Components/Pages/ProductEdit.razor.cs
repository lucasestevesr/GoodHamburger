using GoodHamburger.Web.Products;
using GoodHamburger.Web.Products.Requests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class ProductEdit
{
    [Parameter] public Guid ProductId { get; set; }

    private readonly UpdateProductRequest _request = new() { Category = ProductCategory.Burger, IsActive = true };
    private MudForm? _form;
    private bool _loadingData = true;
    private bool _saving;

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (!Auth.CanManageProducts)
        {
            Snackbar.Add("Seu perfil nao tem permissao para editar itens do cardapio.", Severity.Warning);
            Navigation.NavigateTo("/products");
            return;
        }

        try
        {
            var product = await ProductsApi.GetByIdAsync(ProductId);
            _request.Name = product.Name;
            _request.Price = product.Price;
            _request.Description = product.Description;
            _request.Category = Enum.TryParse<ProductCategory>(product.Category, true, out var category)
                ? category
                : ProductCategory.Burger;
            _request.IsActive = product.IsActive;
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            Navigation.NavigateTo("/products");
        }
        finally
        {
            _loadingData = false;
        }
    }

    private async Task SaveAsync()
    {
        if (_form is null)
            return;

        await _form.ValidateAsync();
        if (!_form.IsValid)
            return;

        try
        {
            _saving = true;
            await ProductsApi.UpdateAsync(ProductId, _request);
            Snackbar.Add("Produto atualizado.", Severity.Success);
            Navigation.NavigateTo("/products");
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            _saving = false;
        }
    }
}
