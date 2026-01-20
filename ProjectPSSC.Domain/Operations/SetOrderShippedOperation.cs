using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class SetOrderShippedOperation
{
    public void Execute(Order order)
    {
        order.SetStatus(OrderStatus.SHIPMENT_CREATED);
    }
}
