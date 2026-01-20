using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Repositories;

public interface IShipmentRepository
{
    Task AddAsync(Shipment shipment, CancellationToken ct);
}
