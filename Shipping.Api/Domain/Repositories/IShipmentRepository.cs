using Shipping.Api.Domain.Models;

namespace Shipping.Api.Domain.Repositories;

public interface IShipmentRepository
{
    Task AddAsync(Shipment shipment, CancellationToken ct);
}
