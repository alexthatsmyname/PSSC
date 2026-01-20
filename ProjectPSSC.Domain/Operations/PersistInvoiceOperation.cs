using ProjectPSSC.Domain.Models;
using ProjectPSSC.Domain.Repositories;

namespace ProjectPSSC.Domain.Operations;

public class PersistInvoiceOperation
{
    public async Task ExecuteAsync(Invoice invoice, IInvoiceRepository repository, CancellationToken ct)
    {
        await repository.AddAsync(invoice, ct);
    }
}
