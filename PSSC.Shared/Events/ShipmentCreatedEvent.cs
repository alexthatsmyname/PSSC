namespace PSSC.Shared.Events;

public class ShipmentCreatedEvent
{
    public Guid ShipmentId { get; set; }
    public Guid OrderId { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}
