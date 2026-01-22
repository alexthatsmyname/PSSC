using Billing.Api.Domain.Models;
using Billing.Api.Domain.Repositories;

namespace Billing.Api.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly BillingDbContext _context;

    public InvoiceRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Invoice invoice, CancellationToken ct)
    {
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync(ct);
    }
}
