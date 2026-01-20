using Microsoft.AspNetCore.Mvc;
using ProjectPSSC.Domain.Exceptions;
using ProjectPSSC.Domain.Workflows;

namespace ProiectPSSC.Api.Controllers;

[ApiController]
[Route("api/shipments")]
public class ShipmentsController : ControllerBase
{
    private readonly CreateShipmentWorkflow _createShipmentWorkflow;

    public ShipmentsController(CreateShipmentWorkflow createShipmentWorkflow)
    {
        _createShipmentWorkflow = createShipmentWorkflow;
    }

    [HttpPost("{orderId:guid}")]
    public async Task<IActionResult> CreateShipment(Guid orderId, CancellationToken ct)
    {
        try
        {
            var shipment = await _createShipmentWorkflow.ExecuteAsync(orderId, ct);

            return CreatedAtAction(nameof(CreateShipment), new { orderId = shipment.OrderId }, new
            {
                shipment.Id,
                shipment.OrderId,
                shipment.TrackingNumber,
                Status = shipment.Status.ToString(),
                shipment.CreatedAt
            });
        }
        catch (InvalidShipmentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
