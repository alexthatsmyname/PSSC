namespace PSSC.Shared.Events;

public class InvoiceGeneratedEvent
{
    public Guid InvoiceId { get; set; }
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OccurredAt { get; set; }
}
