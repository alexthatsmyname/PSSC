using Shipping.Api.Domain.Models;
using Shipping.Api.Domain.Repositories;

namespace Shipping.Api.Infrastructure.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly ShippingDbContext _context;

    public ShipmentRepository(ShippingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Shipment shipment, CancellationToken ct)
    {
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync(ct);
    }
}
