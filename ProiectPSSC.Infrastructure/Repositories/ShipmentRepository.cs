using ProjectPSSC.Domain.Models;
using ProjectPSSC.Domain.Repositories;
using ProiectPSSC.Infrastructure.Persistence;

namespace ProiectPSSC.Infrastructure.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly PsscDbContext _context;

    public ShipmentRepository(PsscDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Shipment shipment, CancellationToken ct)
    {
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync(ct);
    }
}
