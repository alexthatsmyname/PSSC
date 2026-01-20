using ProjectPSSC.Domain.Models;
using ProjectPSSC.Domain.Repositories;

namespace ProjectPSSC.Domain.Operations;

public class PersistShipmentOperation
{
    public async Task ExecuteAsync(Shipment shipment, IShipmentRepository repository, CancellationToken ct)
    {
        await repository.AddAsync(shipment, ct);
    }
}
