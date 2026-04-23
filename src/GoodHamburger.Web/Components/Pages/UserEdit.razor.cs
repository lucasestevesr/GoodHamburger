using GoodHamburger.Web.Identity;
using GoodHamburger.Web.Ui;
using GoodHamburger.Web.Users.Requests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class UserEdit
{
    [Parameter] public Guid UserId { get; set; }

    private readonly UpdateUserRequest _formModel = new() { Role = IdentityRoles.Attendant, IsActive = true };
    private MudForm? _form;
    private bool _loadingData = true;
    private bool _saving;

    private string DisplayName => string.IsNullOrWhiteSpace(_formModel.Name) ? "Conta" : _formModel.Name;

    private string DisplayEmail => string.IsNullOrWhiteSpace(_formModel.Email) ? "email@empresa.com" : _formModel.Email;

    private string DisplayRole => AdminVisuals.TranslateRole(_formModel.Role);

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (!Auth.CanManageUsers)
        {
            Snackbar.Add("A administracao completa de usuarios e restrita ao administrador.", Severity.Warning);
            Navigation.NavigateTo("/users");
            return;
        }

        try
        {
            var user = await UsersApi.GetByIdAsync(UserId);
            _formModel.Name = user.Name;
            _formModel.Email = user.Email;
            _formModel.Role = user.Role;
            _formModel.IsActive = user.IsActive;
            _formModel.Password = null;
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            Navigation.NavigateTo("/users");
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
            await UsersApi.UpdateAsync(UserId, _formModel);
            Snackbar.Add("Usuário atualizado.", Severity.Success);
            Navigation.NavigateTo("/users");
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
