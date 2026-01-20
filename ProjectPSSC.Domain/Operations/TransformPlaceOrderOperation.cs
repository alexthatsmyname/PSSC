using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class TransformPlaceOrderOperation
{
    public PlaceOrderDraft Execute(
        string customerName,
        string customerEmail,
        string shippingAddress,
        IEnumerable<(string productId, string productName, int quantity, decimal unitPrice)> items)
    {
        var draft = new PlaceOrderDraft
        {
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            ShippingAddress = shippingAddress,
            Items = items
                .Select(item => new PlaceOrderItemDraft
                {
                    ProductId = item.productId,
                    ProductName = item.productName,
                    Quantity = item.quantity,
                    UnitPrice = item.unitPrice
                })
                .ToList()
        };

        return draft;
    }
}
