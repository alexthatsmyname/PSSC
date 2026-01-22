using Shipping.Api.Domain.Models;
using PSSC.Shared.Events;

namespace Shipping.Api.Domain.Operations;

public class TransformEventToShipmentOperation
{
    public Shipment Execute(InvoiceGeneratedEvent invoiceEvent)
    {
        var trackingNumber = "TRK-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
        var shipment = new Shipment(invoiceEvent.OrderId, trackingNumber);
        return shipment;
    }
}
