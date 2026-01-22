using Microsoft.AspNetCore.Mvc;
using Orders.Api.Domain.Exceptions;
using Orders.Api.Domain.Repositories;
using Orders.Api.Domain.Workflows;
using Orders.Api.DTOs;
using Orders.Api.Services;

namespace Orders.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly PlaceOrderWorkflow _placeOrderWorkflow;
    private readonly IOrderRepository _orderRepository;
    private readonly BillingApiClient _billingApiClient;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        PlaceOrderWorkflow placeOrderWorkflow,
        IOrderRepository orderRepository,
        BillingApiClient billingApiClient,
        ILogger<OrdersController> logger)
    {
        _placeOrderWorkflow = placeOrderWorkflow;
        _orderRepository = orderRepository;
        _billingApiClient = billingApiClient;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(
        [FromBody] PlaceOrderRequest request,
        CancellationToken ct)
    {
        try
        {
            var items = request.Items.Select(item =>
                (item.ProductId, item.ProductName, item.Quantity, item.UnitPrice));

            // Execute workflow - returns OrderPlacedEvent
            var orderPlacedEvent = await _placeOrderWorkflow.ExecuteAsync(
                request.CustomerName,
                request.CustomerEmail,
                request.ShippingAddress,
                items,
                ct);

            _logger.LogInformation("Order placed successfully. OrderId: {OrderId}", orderPlacedEvent.OrderId);

            // Send event to Billing API (synchronous HTTP call)
            var invoiceEvent = await _billingApiClient.SendOrderPlacedEventAsync(orderPlacedEvent, ct);

            if (invoiceEvent != null)
            {
                _logger.LogInformation("Invoice generated for OrderId: {OrderId}, InvoiceId: {InvoiceId}",
                    orderPlacedEvent.OrderId, invoiceEvent.InvoiceId);
            }

            return CreatedAtAction(nameof(GetOrderById), new { id = orderPlacedEvent.OrderId }, orderPlacedEvent);
        }
        catch (InvalidOrderException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdAsync(id, ct);
        if (order == null)
            return NotFound();

        return Ok(new
        {
            order.Id,
            order.CustomerName,
            order.CustomerEmail,
            order.ShippingAddress,
            order.CreatedAt,
            Status = order.Status.ToString(),
            Items = order.Items.Select(i => new
            {
                i.ProductId,
                i.ProductName,
                i.Quantity,
                i.UnitPrice
            }),
            order.TotalAmount
        });
    }
}
