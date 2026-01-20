using ProjectPSSC.Domain.Exceptions;

namespace ProjectPSSC.Domain.Models;

public class OrderItem
{
    public string ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }

    public OrderItem(string productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new InvalidOrderException("Quantity must be greater than 0.");
        if (unitPrice <= 0)
            throw new InvalidOrderException("Unit price must be greater than 0.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
