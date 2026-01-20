namespace ProjectPSSC.Domain.Models;

public class Invoice
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public decimal TotalAmount { get; private set; }
    public InvoiceStatus Status { get; private set; }

    // EF Core parameterless constructor
    protected Invoice() { }

    public Invoice(Guid orderId, decimal totalAmount)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        TotalAmount = totalAmount;
        IssuedAt = DateTime.UtcNow;
        Status = InvoiceStatus.CREATED;
    }

    public void SetStatus(InvoiceStatus status)
    {
        Status = status;
    }
}
