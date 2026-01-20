using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class CreateOrderOperation
{
    public Order Execute(PlaceOrderDraft draft)
    {
        var items = draft.Items
            .Select(item => new OrderItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice))
            .ToList();

        var order = new Order(draft.CustomerName, draft.CustomerEmail, draft.ShippingAddress, items);

        return order;
    }
}
