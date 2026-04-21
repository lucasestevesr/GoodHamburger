using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<IReadOnlyList<User>> ListAsync(CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
        void Remove(User user);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
