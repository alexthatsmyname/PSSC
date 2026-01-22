using Shipping.Api.Domain.Models;
using Shipping.Api.Domain.Repositories;

namespace Shipping.Api.Domain.Operations;

public class PersistShipmentOperation
{
    public async Task ExecuteAsync(Shipment shipment, IShipmentRepository repository, CancellationToken ct)
    {
        await repository.AddAsync(shipment, ct);
    }
}
