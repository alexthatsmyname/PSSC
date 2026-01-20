using ProjectPSSC.Domain.Models;
using ProjectPSSC.Domain.Operations;
using ProjectPSSC.Domain.Repositories;

namespace ProjectPSSC.Domain.Workflows;

public class GenerateInvoiceWorkflow
{
    private readonly IOrderRepository _orderRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ValidateInvoiceGenerationOperation _validateInvoiceGeneration;
    private readonly TransformOrderToInvoiceOperation _transformOrderToInvoice;
    private readonly PersistInvoiceOperation _persistInvoice;
    private readonly SetOrderInvoicedOperation _setOrderInvoiced;

    public GenerateInvoiceWorkflow(
        IOrderRepository orderRepository,
        IInvoiceRepository invoiceRepository,
        ValidateInvoiceGenerationOperation validateInvoiceGeneration,
        TransformOrderToInvoiceOperation transformOrderToInvoice,
        PersistInvoiceOperation persistInvoice,
        SetOrderInvoicedOperation setOrderInvoiced)
    {
        _orderRepository = orderRepository;
        _invoiceRepository = invoiceRepository;
        _validateInvoiceGeneration = validateInvoiceGeneration;
        _transformOrderToInvoice = transformOrderToInvoice;
        _persistInvoice = persistInvoice;
        _setOrderInvoiced = setOrderInvoiced;
    }

    public async Task<Invoice> ExecuteAsync(Guid orderId, CancellationToken ct)
    {
        // Step 1: Load Order by id
        var order = await _orderRepository.GetByIdAsync(orderId, ct);

        // Step 2: Validate Invoice Generation
        _validateInvoiceGeneration.Execute(order);

        // Step 3: Transform Order to Invoice
        var invoice = _transformOrderToInvoice.Execute(order);

        // Step 4: Persist Invoice
        await _persistInvoice.ExecuteAsync(invoice, _invoiceRepository, ct);

        // Step 5: Set Order Status to INVOICED
        _setOrderInvoiced.Execute(order);

        // Step 6: Update Order in repository
        await _orderRepository.UpdateAsync(order, ct);

        // Step 7: Return Invoice
        return invoice;
    }
}
