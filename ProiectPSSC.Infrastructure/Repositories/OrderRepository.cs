using Microsoft.EntityFrameworkCore;
using ProjectPSSC.Domain.Models;
using ProjectPSSC.Domain.Repositories;
using ProiectPSSC.Infrastructure.Persistence;

namespace ProiectPSSC.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PsscDbContext _context;

    public OrderRepository(PsscDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order, CancellationToken ct)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public async Task UpdateAsync(Order order, CancellationToken ct)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(ct);
    }
}
