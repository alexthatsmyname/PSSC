using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class SetOrderStatusOperation
{
    public void Execute(Order order, OrderStatus status)
    {
        order.SetStatus(status);
    }
}
