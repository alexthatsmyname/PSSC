using Billing.Api.Domain.Models;
using Billing.Api.Domain.Repositories;

namespace Billing.Api.Domain.Operations;

public class PersistInvoiceOperation
{
    public async Task ExecuteAsync(Invoice invoice, IInvoiceRepository repository, CancellationToken ct)
    {
        await repository.AddAsync(invoice, ct);
    }
}
