using GoodHamburger.Web.Orders.Requests;
using GoodHamburger.Web.Products.Responses;
using GoodHamburger.Web.Ui;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class OrderCreate
{
    private readonly List<ProductResponse> _products = [];
    private Guid? _burgerId;
    private Guid? _sideId;
    private Guid? _drinkId;
    private int _burgerQuantity = 1;
    private int _sideQuantity = 1;
    private int _drinkQuantity = 1;
    private bool _loading;
    private bool _loadingProducts = true;

    private IReadOnlyList<OrderItemPreview> SelectedItems =>
        BuildPreviewItems().ToList();

    private bool HasSelectedItems => SelectedItems.Count > 0;

    private decimal EstimatedTotal => SelectedItems.Sum(item => item.LineTotal);

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (!Auth.CanManageOrders)
        {
            Snackbar.Add("Seu perfil nao tem permissao para criar pedidos.", Severity.Warning);
            Navigation.NavigateTo("/orders");
            return;
        }

        try
        {
            _loadingProducts = true;
            _products.AddRange(await ProductsApi.ListAsync());
        }
        finally
        {
            _loadingProducts = false;
        }
    }

    private IEnumerable<ProductResponse> ActiveProducts(string category)
    {
        return _products
            .Where(product => product.IsActive && string.Equals(product.Category, category, StringComparison.OrdinalIgnoreCase))
            .OrderBy(product => product.Name);
    }

    private async Task CreateOrderAsync()
    {
        var items = new List<CreateOrderItemRequest>();
        AddCreateItem(items, _burgerId, _burgerQuantity);
        AddCreateItem(items, _sideId, _sideQuantity);
        AddCreateItem(items, _drinkId, _drinkQuantity);

        try
        {
            _loading = true;
            var created = await OrdersApi.CreateAsync(new CreateOrderRequest { Items = items });
            Snackbar.Add($"Pedido #{created.OrderNumber} criado.", Severity.Success);
            Navigation.NavigateTo($"/orders/{created.Id}");
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

    private static void AddCreateItem(List<CreateOrderItemRequest> items, Guid? productId, int quantity)
    {
        if (productId.HasValue)
            items.Add(new CreateOrderItemRequest { ProductId = productId.Value, Quantity = quantity });
    }

    private IEnumerable<OrderItemPreview> BuildPreviewItems()
    {
        var burger = BuildPreviewItem(_burgerId, _burgerQuantity, "Burger");
        if (burger is not null)
            yield return burger;

        var side = BuildPreviewItem(_sideId, _sideQuantity, "Side");
        if (side is not null)
            yield return side;

        var drink = BuildPreviewItem(_drinkId, _drinkQuantity, "Drink");
        if (drink is not null)
            yield return drink;
    }

    private OrderItemPreview? BuildPreviewItem(Guid? productId, int quantity, string category)
    {
        if (!productId.HasValue)
            return null;

        var product = _products.FirstOrDefault(item => item.Id == productId.Value);
        if (product is null)
            return null;

        return new OrderItemPreview(
            product.Name,
            AdminVisuals.TranslateCategory(category),
            quantity,
            product.Price * quantity);
    }

    private sealed record OrderItemPreview(string Name, string CategoryLabel, int Quantity, decimal LineTotal);
}
