namespace GoodHamburger.Web.Auth.Responses;

public sealed class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public AuthenticatedUserResponse User { get; set; } = new();
}
