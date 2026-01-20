using ProjectPSSC.Domain.Models;
using ProjectPSSC.Domain.Repositories;

namespace ProjectPSSC.Domain.Operations;

public class PersistOrderOperation
{
    public async Task ExecuteAsync(Order order, IOrderRepository repo, CancellationToken ct)
    {
        await repo.AddAsync(order, ct);
    }
}
