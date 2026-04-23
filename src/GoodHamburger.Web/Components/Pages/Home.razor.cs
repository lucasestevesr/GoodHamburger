using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class Home
{
    private int AccessibleModules =>
        (Auth.CanManageOrders ? 1 : 0) +
        1 +
        (Auth.CanManageAttendants ? 1 : 0);

    private string PrimaryModuleLabel =>
        Auth.CanManageOrders ? "Pedidos" :
        Auth.CanManageProducts ? "Cardápio" :
        "Home";

    private string PrimaryModuleIcon =>
        Auth.CanManageOrders ? Icons.Material.Filled.ReceiptLong :
        Auth.CanManageProducts ? Icons.Material.Filled.RestaurantMenu :
        Icons.Material.Filled.SpaceDashboard;

    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated)
            Navigation.NavigateTo("/login", true);
    }
}
