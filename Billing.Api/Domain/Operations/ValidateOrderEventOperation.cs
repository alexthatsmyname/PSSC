using Billing.Api.Domain.Exceptions;
using PSSC.Shared.Events;

namespace Billing.Api.Domain.Operations;

public class ValidateOrderEventOperation
{
    public void Execute(OrderPlacedEvent orderEvent)
    {
        if (orderEvent == null)
            throw new InvalidInvoiceException("Order event must exist.");

        if (orderEvent.OrderId == Guid.Empty)
            throw new InvalidInvoiceException("Order ID is required.");

        if (orderEvent.TotalAmount <= 0)
            throw new InvalidInvoiceException("Order total amount must be greater than 0.");
    }
}
