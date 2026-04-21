namespace GoodHamburger.Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
    }
}
