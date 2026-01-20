using Microsoft.AspNetCore.Mvc;
using ProjectPSSC.Domain.Exceptions;
using ProjectPSSC.Domain.Workflows;

namespace ProiectPSSC.Api.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly GenerateInvoiceWorkflow _generateInvoiceWorkflow;

    public InvoicesController(GenerateInvoiceWorkflow generateInvoiceWorkflow)
    {
        _generateInvoiceWorkflow = generateInvoiceWorkflow;
    }

    [HttpPost("{orderId:guid}")]
    public async Task<IActionResult> GenerateInvoice(Guid orderId, CancellationToken ct)
    {
        try
        {
            var invoice = await _generateInvoiceWorkflow.ExecuteAsync(orderId, ct);

            return CreatedAtAction(nameof(GenerateInvoice), new { orderId = invoice.OrderId }, new
            {
                invoice.Id,
                invoice.OrderId,
                invoice.TotalAmount,
                Status = invoice.Status.ToString(),
                invoice.IssuedAt
            });
        }
        catch (InvalidInvoiceException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
