using Microsoft.EntityFrameworkCore;
using ProjectPSSC.Domain.Models;
using ProjectPSSC.Domain.Repositories;
using ProiectPSSC.Infrastructure.Persistence;

namespace ProiectPSSC.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly PsscDbContext _context;

    public InvoiceRepository(PsscDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Invoice invoice, CancellationToken ct)
    {
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync(ct);
    }
}
