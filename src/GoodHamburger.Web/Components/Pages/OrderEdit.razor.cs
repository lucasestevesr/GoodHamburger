using GoodHamburger.Web.Orders;
using GoodHamburger.Web.Orders.Requests;
using GoodHamburger.Web.Orders.Responses;
using GoodHamburger.Web.Products.Responses;
using GoodHamburger.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class OrderEdit
{
    [Parameter] public Guid OrderId { get; set; }

    private readonly List<ProductResponse> _products = [];
    private OrderResponse? _selectedOrder;
    private Guid _newItemProductId;
    private int _newItemQuantity = 1;
    private bool _loadingData = true;

    private string PageTitle => _selectedOrder is null ? "Detalhes do pedido" : $"Pedido #{_selectedOrder.OrderNumber}";

    private IReadOnlyList<ProductResponse> AvailableProducts => ProductsAvailableForSelectedOrder().ToList();

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (!Auth.CanManageOrders)
        {
            Snackbar.Add("Seu perfil nao tem permissao para editar pedidos.", Severity.Warning);
            Navigation.NavigateTo("/orders");
            return;
        }

        try
        {
            _products.AddRange(await ProductsApi.ListAsync());
            _selectedOrder = await OrdersApi.GetByIdAsync(OrderId);
            SetDefaultNewItem();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            Navigation.NavigateTo("/orders");
        }
        finally
        {
            _loadingData = false;
        }
    }

    private async Task UpdateStatusAsync(OrderStatus status)
    {
        if (_selectedOrder is null)
            return;

        try
        {
            _selectedOrder = await OrdersApi.UpdateStatusAsync(_selectedOrder.Id, status);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task AddItemAsync()
    {
        if (_selectedOrder is null || _newItemProductId == Guid.Empty)
            return;

        try
        {
            _selectedOrder = await OrdersApi.AddItemAsync(_selectedOrder.Id, new AddOrderItemRequest
            {
                ProductId = _newItemProductId,
                Quantity = _newItemQuantity
            });

            Snackbar.Add("Item adicionado.", Severity.Success);
            SetDefaultNewItem();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task UpdateItemQuantityAsync(Guid productId, int quantity)
    {
        if (_selectedOrder is null)
            return;

        try
        {
            _selectedOrder = await OrdersApi.UpdateItemQuantityAsync(_selectedOrder.Id, productId, quantity);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task RemoveItemAsync(Guid productId)
        => await RemoveItemAsync(productId, "este item");

    private async Task RemoveItemAsync(Guid productId, string productName)
    {
        if (_selectedOrder is null)
            return;

        var confirmed = await ConfirmAsync(
            "Remover item",
            $"Deseja remover {productName} deste pedido?");

        if (!confirmed)
            return;

        try
        {
            await OrdersApi.RemoveItemAsync(_selectedOrder.Id, productId);
            _selectedOrder = await OrdersApi.GetByIdAsync(_selectedOrder.Id);
            SetDefaultNewItem();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }

    private IEnumerable<ProductResponse> ProductsAvailableForSelectedOrder()
    {
        if (_selectedOrder is null)
            return [];

        var usedCategories = _selectedOrder.Items.Select(item => item.Category).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return _products
            .Where(product => product.IsActive && !usedCategories.Contains(product.Category))
            .OrderBy(product => product.Category)
            .ThenBy(product => product.Name);
    }

    private void SetDefaultNewItem()
    {
        _newItemProductId = ProductsAvailableForSelectedOrder().FirstOrDefault()?.Id ?? Guid.Empty;
        _newItemQuantity = 1;
    }

    private static OrderStatus ParseStatus(string status)
    {
        return Enum.TryParse<OrderStatus>(status, true, out var parsed) ? parsed : OrderStatus.Pending;
    }

    private static string TranslateCategory(string category)
    {
        return category switch
        {
            "Burger" => "Sanduíche",
            "Side" => "Acompanhamento",
            "Drink" => "Bebida",
            _ => category
        };
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
