using GoodHamburger.Web.Orders.Responses;
using GoodHamburger.Web.Components.Shared;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class Orders
{
    private const string AllStatusFilter = "All";
    private readonly List<OrderSummaryResponse> _orders = [];
    private string _orderNumberFilter = string.Empty;
    private string _statusFilter = AllStatusFilter;
    private bool _loadingData = true;

    private IEnumerable<OrderSummaryResponse> FilteredOrders =>
        _orders
            .Where(order => string.IsNullOrWhiteSpace(_orderNumberFilter)
                || order.OrderNumber.ToString().Contains(_orderNumberFilter, StringComparison.OrdinalIgnoreCase))
            .Where(order => _statusFilter == AllStatusFilter
                || string.Equals(order.Status, _statusFilter, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(order => order.CreationDate);

    private int ProcessingOrders => _orders.Count(order => string.Equals(order.Status, "Processing", StringComparison.OrdinalIgnoreCase));

    private int CompletedOrders => _orders.Count(order => string.Equals(order.Status, "Completed", StringComparison.OrdinalIgnoreCase));

    private decimal AverageTicket => _orders.Count == 0 ? 0 : _orders.Average(order => order.Total);

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (Auth.CanManageOrders)
            await LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            _loadingData = true;
            _orders.Clear();
            _orders.AddRange(await OrdersApi.ListAsync());
        }
        finally
        {
            _loadingData = false;
        }
    }

    private async Task DeleteOrderAsync(OrderSummaryResponse order)
    {
        var confirmed = await ConfirmAsync(
            "Remover pedido",
            $"Deseja remover o pedido #{order.OrderNumber}? Esta acao nao pode ser desfeita.");

        if (!confirmed)
            return;

        try
        {
            await OrdersApi.DeleteAsync(order.Id);
            Snackbar.Add("Pedido removido.", Severity.Success);
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
            [nameof(ConfirmDialog.ConfirmText)] = "Remover",
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
