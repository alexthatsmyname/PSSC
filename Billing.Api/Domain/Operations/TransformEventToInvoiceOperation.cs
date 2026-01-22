using Billing.Api.Domain.Models;
using PSSC.Shared.Events;

namespace Billing.Api.Domain.Operations;

public class TransformEventToInvoiceOperation
{
    public Invoice Execute(OrderPlacedEvent orderEvent)
    {
        var invoice = new Invoice(orderEvent.OrderId, orderEvent.TotalAmount);
        return invoice;
    }
}
