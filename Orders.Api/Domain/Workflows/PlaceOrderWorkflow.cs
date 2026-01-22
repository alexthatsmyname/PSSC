using Orders.Api.Domain.Models;
using Orders.Api.Domain.Operations;
using Orders.Api.Domain.Repositories;
using PSSC.Shared.Events;

namespace Orders.Api.Domain.Workflows;

public class PlaceOrderWorkflow
{
    private readonly TransformPlaceOrderOperation _transform;
    private readonly ValidatePlaceOrderOperation _validate;
    private readonly CreateOrderOperation _create;
    private readonly SetOrderStatusOperation _setStatus;
    private readonly PersistOrderOperation _persist;
    private readonly IOrderRepository _repository;

    public PlaceOrderWorkflow(
        TransformPlaceOrderOperation transform,
        ValidatePlaceOrderOperation validate,
        CreateOrderOperation create,
        SetOrderStatusOperation setStatus,
        PersistOrderOperation persist,
        IOrderRepository repository)
    {
        _transform = transform;
        _validate = validate;
        _create = create;
        _setStatus = setStatus;
        _persist = persist;
        _repository = repository;
    }

    public async Task<OrderPlacedEvent> ExecuteAsync(
        string customerName,
        string customerEmail,
        string shippingAddress,
        IEnumerable<(string productId, string productName, int quantity, decimal unitPrice)> items,
        CancellationToken ct)
    {
        // Step 1: Transform
        var draft = _transform.Execute(customerName, customerEmail, shippingAddress, items);

        // Step 2: Validate
        _validate.Execute(draft);

        // Step 3: Create
        var order = _create.Execute(draft);

        // Step 4: Set Status
        _setStatus.Execute(order, OrderStatus.VALIDATED);

        // Step 5: Persist
        await _persist.ExecuteAsync(order, _repository, ct);

        // Step 6: Generate and Return Event
        return new OrderPlacedEvent
        {
            OrderId = order.Id,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            ShippingAddress = order.ShippingAddress,
            TotalAmount = order.TotalAmount,
            OccurredAt = DateTime.UtcNow
        };
    }
}
