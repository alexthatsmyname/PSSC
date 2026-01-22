using Microsoft.AspNetCore.Mvc;
using Billing.Api.Domain.Exceptions;
using Billing.Api.Domain.Workflows;
using Billing.Api.Services;
using PSSC.Shared.Events;

namespace Billing.Api.Controllers;

[ApiController]
[Route("api/billing")]
public class BillingController : ControllerBase
{
    private readonly GenerateInvoiceWorkflow _generateInvoiceWorkflow;
    private readonly ShippingApiClient _shippingApiClient;
    private readonly ILogger<BillingController> _logger;

    public BillingController(
        GenerateInvoiceWorkflow generateInvoiceWorkflow,
        ShippingApiClient shippingApiClient,
        ILogger<BillingController> logger)
    {
        _generateInvoiceWorkflow = generateInvoiceWorkflow;
        _shippingApiClient = shippingApiClient;
        _logger = logger;
    }

    [HttpPost("events/order-placed")]
    public async Task<IActionResult> HandleOrderPlacedEvent(
        [FromBody] OrderPlacedEvent orderEvent,
        CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Received OrderPlacedEvent for OrderId: {OrderId}", orderEvent.OrderId);

            // Execute workflow - returns InvoiceGeneratedEvent
            var invoiceGeneratedEvent = await _generateInvoiceWorkflow.ExecuteAsync(orderEvent, ct);

            _logger.LogInformation("Invoice generated successfully. InvoiceId: {InvoiceId}, OrderId: {OrderId}",
                invoiceGeneratedEvent.InvoiceId, invoiceGeneratedEvent.OrderId);

            // Send event to Shipping API (synchronous HTTP call)
            var shipmentEvent = await _shippingApiClient.SendInvoiceGeneratedEventAsync(invoiceGeneratedEvent, ct);

            if (shipmentEvent != null)
            {
                _logger.LogInformation("Shipment created for OrderId: {OrderId}, ShipmentId: {ShipmentId}, TrackingNumber: {TrackingNumber}",
                    invoiceGeneratedEvent.OrderId, shipmentEvent.ShipmentId, shipmentEvent.TrackingNumber);
            }

            return Ok(invoiceGeneratedEvent);
        }
        catch (InvalidInvoiceException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
