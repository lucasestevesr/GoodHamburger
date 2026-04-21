using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Application.Auth.Interfaces
{
    public interface IAccessTokenService
    {
        string GenerateToken(User user);

        int GetExpiresInSeconds();
    }
}
