namespace ProjectPSSC.Domain.Models;

public class PlaceOrderResult
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
}
