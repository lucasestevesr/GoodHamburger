using GoodHamburger.Application.Users.Responses;
using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Application.Users.Mappings
{
    internal static class UserMappings
    {
        internal static UserResponse ToResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreationDate = user.CreationDate
            };
        }
    }
}
