using GoodHamburger.Web.Products;
using GoodHamburger.Web.Products.Requests;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class ProductCreate
{
    private readonly CreateProductRequest _request = new() { IsActive = true, Category = ProductCategory.Burger };
    private MudForm? _form;
    private bool _loading;

    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (!Auth.CanManageProducts)
        {
            Snackbar.Add("Seu perfil nao tem permissao para cadastrar itens do cardapio.", Severity.Warning);
            Navigation.NavigateTo("/products");
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
            _loading = true;
            await ProductsApi.CreateAsync(_request);
            Snackbar.Add("Produto criado.", Severity.Success);
            Navigation.NavigateTo("/products");
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }
}
