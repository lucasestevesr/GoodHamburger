using GoodHamburger.Application.Identity.Responses;
using GoodHamburger.Application.Users.Responses;

namespace GoodHamburger.Application.Users.Mappings
{
    internal static class UserMappings
    {
        internal static UserResponse ToResponse(this IdentityUserResponse user, string role)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = role,
                IsActive = user.IsActive,
                CreationDate = user.CreationDate
            };
        }
    }
}
