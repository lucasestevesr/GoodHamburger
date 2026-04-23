namespace GoodHamburger.Web.Identity;

public static class IdentityRoles
{
    public const string Admin = nameof(Admin);
    public const string Manager = nameof(Manager);
    public const string Attendant = nameof(Attendant);

    public static IReadOnlyCollection<string> All { get; } = [Admin, Manager, Attendant];
}
