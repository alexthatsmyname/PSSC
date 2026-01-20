using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(Order order, CancellationToken ct);
}
