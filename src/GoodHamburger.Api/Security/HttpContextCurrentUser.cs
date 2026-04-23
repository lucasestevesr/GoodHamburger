using System.Security.Claims;
using GoodHamburger.Application.Common.Interfaces;

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

        public string? Role => User?.FindFirstValue(ClaimTypes.Role);

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
