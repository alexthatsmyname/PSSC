using Billing.Api.Domain.Models;
using Billing.Api.Domain.Operations;
using Billing.Api.Domain.Repositories;
using PSSC.Shared.Events;

namespace Billing.Api.Domain.Workflows;

public class GenerateInvoiceWorkflow
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ValidateOrderEventOperation _validateOrderEvent;
    private readonly TransformEventToInvoiceOperation _transformEventToInvoice;
    private readonly PersistInvoiceOperation _persistInvoice;

    public GenerateInvoiceWorkflow(
        IInvoiceRepository invoiceRepository,
        ValidateOrderEventOperation validateOrderEvent,
        TransformEventToInvoiceOperation transformEventToInvoice,
        PersistInvoiceOperation persistInvoice)
    {
        _invoiceRepository = invoiceRepository;
        _validateOrderEvent = validateOrderEvent;
        _transformEventToInvoice = transformEventToInvoice;
        _persistInvoice = persistInvoice;
    }

    public async Task<InvoiceGeneratedEvent> ExecuteAsync(OrderPlacedEvent orderEvent, CancellationToken ct)
    {
        // Step 1: Validate Order Event
        _validateOrderEvent.Execute(orderEvent);

        // Step 2: Transform Event to Invoice
        var invoice = _transformEventToInvoice.Execute(orderEvent);

        // Step 3: Persist Invoice
        await _persistInvoice.ExecuteAsync(invoice, _invoiceRepository, ct);

        // Step 4: Generate and Return Event
        return new InvoiceGeneratedEvent
        {
            InvoiceId = invoice.Id,
            OrderId = invoice.OrderId,
            TotalAmount = invoice.TotalAmount,
            OccurredAt = DateTime.UtcNow
        };
    }
}
