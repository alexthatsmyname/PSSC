using Microsoft.AspNetCore.Mvc;
using ProjectPSSC.Domain.Exceptions;
using ProjectPSSC.Domain.Repositories;
using ProjectPSSC.Domain.Workflows;
using ProiectPSSC.Api.DTOs;

namespace ProiectPSSC.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly PlaceOrderWorkflow _placeOrderWorkflow;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(PlaceOrderWorkflow placeOrderWorkflow, IOrderRepository orderRepository)
    {
        _placeOrderWorkflow = placeOrderWorkflow;
        _orderRepository = orderRepository;
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

            var result = await _placeOrderWorkflow.ExecuteAsync(
                request.CustomerName,
                request.CustomerEmail,
                request.ShippingAddress,
                items,
                ct);

            return CreatedAtAction(nameof(GetOrderById), new { id = result.OrderId }, new
            {
                result.OrderId,
                Status = result.Status.ToString(),
                result.TotalAmount
            });
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
