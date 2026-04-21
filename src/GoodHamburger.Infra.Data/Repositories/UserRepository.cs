using GoodHamburger.Domain.Entities.Auth;
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

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
        {
            return await context.Users.AnyAsync(
                user => user.Email.ToLower() == email.ToLower(),
                ct);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await context.Users.SingleOrDefaultAsync(user => user.Id == id, ct);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        {
            return await context.Users.SingleOrDefaultAsync(
                user => user.Email.ToLower() == email.ToLower(),
                ct);
        }

        public async Task<IReadOnlyList<User>> ListAsync(CancellationToken ct)
        {
            return await context.Users
                .AsNoTracking()
                .OrderBy(user => user.Name)
                .ToListAsync(ct);
        }

        public async Task AddAsync(User user, CancellationToken ct)
        {
            await context.Users.AddAsync(user, ct);
        }

        public void Remove(User user)
        {
            context.Users.Remove(user);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await context.SaveChangesAsync(ct);
        }
    }
}
