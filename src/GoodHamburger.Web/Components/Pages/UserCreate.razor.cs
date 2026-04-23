using GoodHamburger.Web.Identity;
using GoodHamburger.Web.Ui;
using GoodHamburger.Web.Users.Requests;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class UserCreate
{
    private readonly UpdateUserRequest _formModel = new() { Role = IdentityRoles.Attendant, IsActive = true };
    private MudForm? _form;
    private bool _loading;

    private string DisplayName => string.IsNullOrWhiteSpace(_formModel.Name) ? "Nova conta" : _formModel.Name;

    private string DisplayEmail => string.IsNullOrWhiteSpace(_formModel.Email) ? "email@empresa.com" : _formModel.Email;

    private string DisplayRole => AdminVisuals.TranslateRole(_formModel.Role);

    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (!Auth.CanManageAttendants)
        {
            Snackbar.Add("Seu perfil nao tem permissao para criar usuarios.", Severity.Warning);
            Navigation.NavigateTo("/users");
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
            if (Auth.IsAdmin)
            {
                await UsersApi.CreateAsync(new CreateUserRequest
                {
                    Name = _formModel.Name,
                    Email = _formModel.Email,
                    Password = _formModel.Password ?? string.Empty,
                    Role = _formModel.Role,
                    IsActive = _formModel.IsActive
                });
            }
            else
            {
                await UsersApi.CreateAttendantAsync(new CreateAttendantRequest
                {
                    Name = _formModel.Name,
                    Email = _formModel.Email,
                    Password = _formModel.Password ?? string.Empty,
                    IsActive = _formModel.IsActive
                });
            }

            Snackbar.Add("Usuário criado.", Severity.Success);
            Navigation.NavigateTo("/users", replace: true);
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
