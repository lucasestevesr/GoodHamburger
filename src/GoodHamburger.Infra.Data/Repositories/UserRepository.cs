using GoodHamburger.Application.Users.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infra.Data.Repositories
{
    public sealed class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct)
        {
            return await context.Users.AnyAsync(user => user.Id == id, ct);
        }
    }
}
