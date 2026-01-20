using ProjectPSSC.Domain.Exceptions;
using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class ValidateInvoiceGenerationOperation
{
    public void Execute(Order order)
    {
        if (order == null)
            throw new InvalidInvoiceException("Order must exist.");

        if (order.Status != OrderStatus.VALIDATED)
            throw new InvalidInvoiceException("Order must be in VALIDATED status to generate an invoice.");
    }
}
