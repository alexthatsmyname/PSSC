namespace Orders.Api.Domain.Models;

public class Order
{
    public Guid Id { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public string CustomerEmail { get; private set; } = string.Empty;
    public string ShippingAddress { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public List<OrderItem> Items { get; private set; } = new();
    public decimal TotalAmount { get; private set; }

    protected Order() { }

    public Order(string customerName, string customerEmail, string shippingAddress, IEnumerable<OrderItem> items)
    {
        Id = Guid.NewGuid();
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        ShippingAddress = shippingAddress;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.CREATED;
        Items = items.ToList();
        TotalAmount = Items.Sum(item => item.Quantity * item.UnitPrice);
    }

    public void SetStatus(OrderStatus status)
    {
        Status = status;
    }

    public IReadOnlyCollection<OrderItem> GetItems() => Items.AsReadOnly();
}
