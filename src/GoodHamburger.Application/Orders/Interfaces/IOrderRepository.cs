using GoodHamburger.Domain.Entities.Orders;

namespace GoodHamburger.Application.Orders.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Order>> ListAsync(CancellationToken ct);
        Task<long> GetNextOrderNumberAsync(CancellationToken ct);
        Task AddAsync(Order order, CancellationToken ct);
        void Remove(Order order);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
