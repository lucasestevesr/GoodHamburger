namespace GoodHamburger.Application.Auth.Interfaces
{
    public interface IAccessTokenService
    {
        string GenerateAccessToken(Guid userId, string? email, IEnumerable<string> roles);

        int GetExpiresInSeconds();
    }
}
