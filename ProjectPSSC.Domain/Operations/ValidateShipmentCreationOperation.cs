using ProjectPSSC.Domain.Exceptions;
using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class ValidateShipmentCreationOperation
{
    public void Execute(Order order)
    {
        if (order == null)
            throw new InvalidShipmentException("Order must exist.");

        if (order.Status != OrderStatus.INVOICED)
            throw new InvalidShipmentException("Order must be in INVOICED status to create a shipment.");
    }
}
