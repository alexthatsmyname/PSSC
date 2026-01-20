using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Repositories;

public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice, CancellationToken ct);
}
