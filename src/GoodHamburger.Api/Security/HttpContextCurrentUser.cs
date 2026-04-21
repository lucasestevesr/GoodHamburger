using System.Security.Claims;
using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Api.Security
{
    public sealed class HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

        public Guid? UserId
        {
            get
            {
                var value = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User?.FindFirstValue("sub");

                return Guid.TryParse(value, out var userId) ? userId : null;
            }
        }

        public string? Email => User?.FindFirstValue(ClaimTypes.Email);

        public UserRole? Role
        {
            get
            {
                var value = User?.FindFirstValue(ClaimTypes.Role);
                return Enum.TryParse<UserRole>(value, out var role) ? role : null;
            }
        }

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
