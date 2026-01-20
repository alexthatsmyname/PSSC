namespace ProjectPSSC.Domain.Models;

public class Shipment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string TrackingNumber { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public ShipmentStatus Status { get; private set; }

    protected Shipment() { }

    public Shipment(Guid orderId, string trackingNumber)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        TrackingNumber = trackingNumber;
        CreatedAt = DateTime.UtcNow;
        Status = ShipmentStatus.CREATED;
    }
}
