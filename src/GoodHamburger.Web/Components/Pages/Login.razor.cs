using GoodHamburger.Web.Auth.Requests;
using GoodHamburger.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class Login
{
    [Inject] private AuthSessionStorage AuthStorage { get; set; } = default!;

    private readonly LoginRequest _request = new();
    private MudForm? _form;
    private bool _loading;

    private async Task LoginAsync()
    {
        if (_form is null)
            return;

        await _form.ValidateAsync();
        if (!_form.IsValid)
            return;

        try
        {
            _loading = true;
            var response = await AuthApi.LoginAsync(_request);
            await Auth.SignInAsync(response, AuthStorage);
            Snackbar.Add($"Bem-vindo, {response.User.Name}.", Severity.Success);
            Navigation.NavigateTo("/");
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
