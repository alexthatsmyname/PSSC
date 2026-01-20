using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class SetOrderInvoicedOperation
{
    public void Execute(Order order)
    {
        order.SetStatus(OrderStatus.INVOICED);
    }
}
