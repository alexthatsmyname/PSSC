using Orders.Api.Domain.Exceptions;
using Orders.Api.Domain.Models;

namespace Orders.Api.Domain.Operations;

public class ValidatePlaceOrderOperation
{
    public void Execute(PlaceOrderDraft draft)
    {
        if (string.IsNullOrWhiteSpace(draft.CustomerName))
            throw new InvalidOrderException("Customer name is required.");

        if (string.IsNullOrWhiteSpace(draft.CustomerEmail) || !draft.CustomerEmail.Contains("@"))
            throw new InvalidOrderException("A valid customer email is required.");

        if (string.IsNullOrWhiteSpace(draft.ShippingAddress))
            throw new InvalidOrderException("Shipping address is required.");

        if (draft.Items == null || draft.Items.Count == 0)
            throw new InvalidOrderException("At least one item is required.");

        foreach (var item in draft.Items)
        {
            if (item.Quantity <= 0)
                throw new InvalidOrderException("Item quantity must be greater than 0.");
            if (item.UnitPrice <= 0)
                throw new InvalidOrderException("Item unit price must be greater than 0.");
        }
    }
}
