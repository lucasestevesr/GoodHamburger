namespace GoodHamburger.Web.Components.Pages;

public partial class Home
{
    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated)
            Navigation.NavigateTo("/login", true);
    }
}
