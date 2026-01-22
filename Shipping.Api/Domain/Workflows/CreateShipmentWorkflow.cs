using Shipping.Api.Domain.Models;
using Shipping.Api.Domain.Operations;
using Shipping.Api.Domain.Repositories;
using PSSC.Shared.Events;

namespace Shipping.Api.Domain.Workflows;

public class CreateShipmentWorkflow
{
    private readonly IShipmentRepository _shipmentRepository;
    private readonly ValidateInvoiceEventOperation _validateInvoiceEvent;
    private readonly TransformEventToShipmentOperation _transformEventToShipment;
    private readonly PersistShipmentOperation _persistShipment;

    public CreateShipmentWorkflow(
        IShipmentRepository shipmentRepository,
        ValidateInvoiceEventOperation validateInvoiceEvent,
        TransformEventToShipmentOperation transformEventToShipment,
        PersistShipmentOperation persistShipment)
    {
        _shipmentRepository = shipmentRepository;
        _validateInvoiceEvent = validateInvoiceEvent;
        _transformEventToShipment = transformEventToShipment;
        _persistShipment = persistShipment;
    }

    public async Task<ShipmentCreatedEvent> ExecuteAsync(InvoiceGeneratedEvent invoiceEvent, CancellationToken ct)
    {
        // Step 1: Validate Invoice Event
        _validateInvoiceEvent.Execute(invoiceEvent);

        // Step 2: Transform Event to Shipment
        var shipment = _transformEventToShipment.Execute(invoiceEvent);

        // Step 3: Persist Shipment
        await _persistShipment.ExecuteAsync(shipment, _shipmentRepository, ct);

        // Step 4: Generate and Return Event
        return new ShipmentCreatedEvent
        {
            ShipmentId = shipment.Id,
            OrderId = shipment.OrderId,
            TrackingNumber = shipment.TrackingNumber,
            OccurredAt = DateTime.UtcNow
        };
    }
}
