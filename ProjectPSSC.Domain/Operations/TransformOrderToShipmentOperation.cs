using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class TransformOrderToShipmentOperation
{
    public Shipment Execute(Order order)
    {
        var trackingNumber = "TRK-" + Guid.NewGuid();
        var shipment = new Shipment(order.Id, trackingNumber);
        return shipment;
    }
}
