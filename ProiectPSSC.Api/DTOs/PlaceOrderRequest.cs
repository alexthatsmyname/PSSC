namespace ProiectPSSC.Api.DTOs;

public class PlaceOrderRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public List<PlaceOrderItemRequest> Items { get; set; } = new();
}
