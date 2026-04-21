using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }

        string? Email { get; }

        UserRole? Role { get; }

        bool IsAuthenticated { get; }
    }
}
