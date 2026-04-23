using GoodHamburger.Web.Products.Responses;
using GoodHamburger.Web.Components.Shared;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class Products
{
    private const string AllCategoryFilter = "All";
    private const string AllAvailabilityFilter = "all";
    private readonly List<ProductResponse> _products = [];
    private string _search = string.Empty;
    private string _categoryFilter = AllCategoryFilter;
    private string _availabilityFilter = AllAvailabilityFilter;
    private bool _loadingData = true;

    private IEnumerable<ProductResponse> FilteredProducts =>
        _products
            .Where(product => string.IsNullOrWhiteSpace(_search)
                || product.Name.Contains(_search, StringComparison.OrdinalIgnoreCase)
                || (product.Description?.Contains(_search, StringComparison.OrdinalIgnoreCase) ?? false))
            .Where(product => _categoryFilter == AllCategoryFilter
                || string.Equals(product.Category, _categoryFilter, StringComparison.OrdinalIgnoreCase))
            .Where(product => _availabilityFilter == AllAvailabilityFilter
                || (_availabilityFilter == "active" && product.IsActive)
                || (_availabilityFilter == "inactive" && !product.IsActive))
            .OrderBy(product => product.Category)
            .ThenBy(product => product.Name);

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated || !Auth.CanViewProducts)
        {
            _loadingData = false;
            return;
        }

        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            _loadingData = true;
            _products.Clear();
            _products.AddRange(await ProductsApi.ListAsync());
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            _loadingData = false;
        }
    }

    private async Task ToggleStatusAsync(ProductResponse product)
    {
        try
        {
            await ProductsApi.UpdateStatusAsync(product.Id, !product.IsActive);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task DeleteAsync(ProductResponse product)
    {
        var confirmed = await ConfirmAsync(
            "Excluir item",
            $"Deseja excluir \"{product.Name}\" do cardapio? Esta acao nao pode ser desfeita.");

        if (!confirmed)
            return;

        try
        {
            await ProductsApi.DeleteAsync(product.Id);
            Snackbar.Add("Produto removido.", Severity.Success);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task<bool> ConfirmAsync(string title, string message)
    {
        var parameters = new DialogParameters
        {
            [nameof(ConfirmDialog.Title)] = title,
            [nameof(ConfirmDialog.Message)] = message,
            [nameof(ConfirmDialog.ConfirmText)] = "Excluir",
            [nameof(ConfirmDialog.ConfirmColor)] = Color.Error
        };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>(title, parameters, new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraSmall,
            FullWidth = true
        });

        var result = await dialog.Result;
        return result is not null && !result.Canceled;
    }
}
