using Orders.Api.Domain.Models;

namespace Orders.Api.Domain.Operations;

public class SetOrderStatusOperation
{
    public void Execute(Order order, OrderStatus status)
    {
        order.SetStatus(status);
    }
}
