using GoodHamburger.Application.Orders.Interfaces;
using GoodHamburger.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infra.Data.Repositories
{
    public sealed class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await context.Orders
                .Include(order => order.Items)
                    .ThenInclude(item => item.Product)
                .SingleOrDefaultAsync(order => order.Id == id, ct);
        }

        public async Task<IReadOnlyList<Order>> ListAsync(CancellationToken ct)
        {
            return await context.Orders
                .AsNoTracking()
                .Include(order => order.Items)
                    .ThenInclude(item => item.Product)
                .OrderByDescending(order => order.OrderNumber)
                .ToListAsync(ct);
        }

        public async Task<long> GetNextOrderNumberAsync(CancellationToken ct)
        {
            var lastOrderNumber = await context.Orders
                .Select(order => (long?)order.OrderNumber)
                .MaxAsync(ct);

            return (lastOrderNumber ?? 0) + 1;
        }

        public async Task AddAsync(Order order, CancellationToken ct)
        {
            await context.Orders.AddAsync(order, ct);
        }

        public void Remove(Order order)
        {
            context.Orders.Remove(order);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await context.SaveChangesAsync(ct);
        }
    }
}
