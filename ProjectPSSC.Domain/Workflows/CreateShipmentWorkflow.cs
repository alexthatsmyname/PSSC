using ProjectPSSC.Domain.Models;
using ProjectPSSC.Domain.Operations;
using ProjectPSSC.Domain.Repositories;

namespace ProjectPSSC.Domain.Workflows;

public class CreateShipmentWorkflow
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShipmentRepository _shipmentRepository;
    private readonly ValidateShipmentCreationOperation _validateShipmentCreation;
    private readonly TransformOrderToShipmentOperation _transformOrderToShipment;
    private readonly PersistShipmentOperation _persistShipment;
    private readonly SetOrderShippedOperation _setOrderShipped;

    public CreateShipmentWorkflow(
        IOrderRepository orderRepository,
        IShipmentRepository shipmentRepository,
        ValidateShipmentCreationOperation validateShipmentCreation,
        TransformOrderToShipmentOperation transformOrderToShipment,
        PersistShipmentOperation persistShipment,
        SetOrderShippedOperation setOrderShipped)
    {
        _orderRepository = orderRepository;
        _shipmentRepository = shipmentRepository;
        _validateShipmentCreation = validateShipmentCreation;
        _transformOrderToShipment = transformOrderToShipment;
        _persistShipment = persistShipment;
        _setOrderShipped = setOrderShipped;
    }

    public async Task<Shipment> ExecuteAsync(Guid orderId, CancellationToken ct)
    {
        // Step 1: Load Order by id
        var order = await _orderRepository.GetByIdAsync(orderId, ct);

        // Step 2: Validate Shipment Creation
        _validateShipmentCreation.Execute(order);

        // Step 3: Transform Order to Shipment
        var shipment = _transformOrderToShipment.Execute(order);

        // Step 4: Persist Shipment
        await _persistShipment.ExecuteAsync(shipment, _shipmentRepository, ct);

        // Step 5: Set Order Status to SHIPMENT_CREATED
        _setOrderShipped.Execute(order);

        // Step 6: Update Order in repository
        await _orderRepository.UpdateAsync(order, ct);

        // Step 7: Return Shipment
        return shipment;
    }
}
