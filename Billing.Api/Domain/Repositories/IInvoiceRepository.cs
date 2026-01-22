using Billing.Api.Domain.Models;

namespace Billing.Api.Domain.Repositories;

public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice, CancellationToken ct);
}
