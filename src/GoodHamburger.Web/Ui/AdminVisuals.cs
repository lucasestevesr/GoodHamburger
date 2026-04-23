using GoodHamburger.Web.Products;
using MudBlazor;

namespace GoodHamburger.Web.Ui;

public static class AdminVisuals
{
    public const string BrandName = "GoodHamburger";

    public static string TranslateOrderStatus(string status)
    {
        return status switch
        {
            "Pending" => "Pendente",
            "Processing" => "Em preparo",
            "Completed" => "Concluído",
            "Cancelled" => "Cancelado",
            _ => status
        };
    }

    public static Color GetOrderStatusColor(string status)
    {
        return status switch
        {
            "Pending" => Color.Warning,
            "Processing" => Color.Info,
            "Completed" => Color.Success,
            "Cancelled" => Color.Error,
            _ => Color.Default
        };
    }

    public static string TranslateCategory(string category)
    {
        return category switch
        {
            "Burger" => "Sanduíche",
            "Side" => "Acompanhamento",
            "Drink" => "Bebida",
            _ => category
        };
    }

    public static string TranslateCategory(ProductCategory category)
        => TranslateCategory(category.ToString());

    public static Color GetCategoryColor(string category)
    {
        return category switch
        {
            "Burger" => Color.Primary,
            "Side" => Color.Warning,
            "Drink" => Color.Info,
            _ => Color.Default
        };
    }

    public static string TranslateRole(string role)
    {
        return role switch
        {
            "Admin" => "Administrador",
            "Manager" => "Gerente",
            "Attendant" => "Atendente",
            _ => role
        };
    }

    public static Color GetRoleColor(string role)
    {
        return role switch
        {
            "Admin" => Color.Error,
            "Manager" => Color.Warning,
            "Attendant" => Color.Info,
            _ => Color.Default
        };
    }

    public static string GetAvailabilityText(bool isActive)
        => isActive ? "Disponível" : "Indisponível";

    public static Color GetAvailabilityColor(bool isActive)
        => isActive ? Color.Success : Color.Default;

    public static string GetInitials(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "GH";

        var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 1)
            return parts[0][..Math.Min(2, parts[0].Length)].ToUpperInvariant();

        return string.Concat(parts.Take(2).Select(part => char.ToUpperInvariant(part[0])));
    }

    public static string FormatDate(DateTimeOffset value)
        => value.ToLocalTime().ToString("dd/MM/yyyy 'as' HH:mm");
}
