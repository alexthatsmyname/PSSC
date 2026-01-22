using Orders.Api.Domain.Models;
using Orders.Api.Domain.Repositories;

namespace Orders.Api.Domain.Operations;

public class PersistOrderOperation
{
    public async Task ExecuteAsync(Order order, IOrderRepository repo, CancellationToken ct)
    {
        await repo.AddAsync(order, ct);
    }
}
