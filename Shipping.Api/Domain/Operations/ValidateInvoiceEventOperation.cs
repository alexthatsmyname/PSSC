using Shipping.Api.Domain.Exceptions;
using PSSC.Shared.Events;

namespace Shipping.Api.Domain.Operations;

public class ValidateInvoiceEventOperation
{
    public void Execute(InvoiceGeneratedEvent invoiceEvent)
    {
        if (invoiceEvent == null)
            throw new InvalidShipmentException("Invoice event must exist.");

        if (invoiceEvent.OrderId == Guid.Empty)
            throw new InvalidShipmentException("Order ID is required.");

        if (invoiceEvent.InvoiceId == Guid.Empty)
            throw new InvalidShipmentException("Invoice ID is required.");
    }
}
