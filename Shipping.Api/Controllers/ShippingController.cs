using Microsoft.AspNetCore.Mvc;
using Shipping.Api.Domain.Exceptions;
using Shipping.Api.Domain.Workflows;
using PSSC.Shared.Events;

namespace Shipping.Api.Controllers;

[ApiController]
[Route("api/shipping")]
public class ShippingController : ControllerBase
{
    private readonly CreateShipmentWorkflow _createShipmentWorkflow;
    private readonly ILogger<ShippingController> _logger;

    public ShippingController(
        CreateShipmentWorkflow createShipmentWorkflow,
        ILogger<ShippingController> logger)
    {
        _createShipmentWorkflow = createShipmentWorkflow;
        _logger = logger;
    }

    [HttpPost("events/invoice-generated")]
    public async Task<IActionResult> HandleInvoiceGeneratedEvent(
        [FromBody] InvoiceGeneratedEvent invoiceEvent,
        CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Received InvoiceGeneratedEvent for OrderId: {OrderId}, InvoiceId: {InvoiceId}",
                invoiceEvent.OrderId, invoiceEvent.InvoiceId);

            // Execute workflow - returns ShipmentCreatedEvent
            var shipmentCreatedEvent = await _createShipmentWorkflow.ExecuteAsync(invoiceEvent, ct);

            _logger.LogInformation("Shipment created successfully. ShipmentId: {ShipmentId}, OrderId: {OrderId}, TrackingNumber: {TrackingNumber}",
                shipmentCreatedEvent.ShipmentId, shipmentCreatedEvent.OrderId, shipmentCreatedEvent.TrackingNumber);

            // OUTPUT EVENT TO CONSOLE (Final step in the chain)
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("       SHIPMENT CREATED EVENT           ");
            Console.WriteLine("========================================");
            Console.WriteLine($"  Shipment ID:     {shipmentCreatedEvent.ShipmentId}");
            Console.WriteLine($"  Order ID:        {shipmentCreatedEvent.OrderId}");
            Console.WriteLine($"  Tracking Number: {shipmentCreatedEvent.TrackingNumber}");
            Console.WriteLine($"  Occurred At:     {shipmentCreatedEvent.OccurredAt:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine("========================================");
            Console.WriteLine();

            return Ok(shipmentCreatedEvent);
        }
        catch (InvalidShipmentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
