using Orders.Api.Domain.Models;

namespace Orders.Api.Domain.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
}
