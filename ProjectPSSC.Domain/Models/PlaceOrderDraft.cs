namespace ProjectPSSC.Domain.Models;

public class PlaceOrderDraft
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public List<PlaceOrderItemDraft> Items { get; set; } = new();
}
