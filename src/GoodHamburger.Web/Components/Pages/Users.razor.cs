using GoodHamburger.Web.Users.Responses;
using GoodHamburger.Web.Components.Shared;
using MudBlazor;

namespace GoodHamburger.Web.Components.Pages;

public partial class Users
{
    private const string AllRolesFilter = "All";
    private const string AllStatusFilter = "all";
    private readonly List<UserResponse> _users = [];
    private string _search = string.Empty;
    private string _roleFilter = AllRolesFilter;
    private string _statusFilter = AllStatusFilter;
    private bool _loadingData = true;
    private string? _loadErrorMessage;

    private IEnumerable<UserResponse> FilteredUsers =>
        _users
            .Where(user => string.IsNullOrWhiteSpace(_search)
                || user.Name.Contains(_search, StringComparison.OrdinalIgnoreCase)
                || user.Email.Contains(_search, StringComparison.OrdinalIgnoreCase))
            .Where(user => _roleFilter == AllRolesFilter
                || string.Equals(user.Role, _roleFilter, StringComparison.OrdinalIgnoreCase))
            .Where(user => _statusFilter == AllStatusFilter
                || (_statusFilter == "active" && user.IsActive)
                || (_statusFilter == "inactive" && !user.IsActive))
            .OrderBy(user => user.Name);

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (!Auth.CanManageAttendants)
        {
            Navigation.NavigateTo("/401");
            return;
        }

        if (Auth.IsAdmin)
            await LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            _loadingData = true;
            _loadErrorMessage = null;
            _users.Clear();
            _users.AddRange(await UsersApi.ListAsync());
        }
        catch (Exception ex)
        {
            _loadErrorMessage = ex.Message;
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            _loadingData = false;
        }
    }

    private async Task DeleteAsync(UserResponse user)
    {
        var confirmed = await ConfirmAsync(
            "Excluir usuario",
            $"Deseja remover a conta de {user.Name}? Esta acao nao pode ser desfeita.");

        if (!confirmed)
            return;

        try
        {
            await UsersApi.DeleteAsync(user.Id);
            Snackbar.Add("Usuário removido.", Severity.Success);
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
