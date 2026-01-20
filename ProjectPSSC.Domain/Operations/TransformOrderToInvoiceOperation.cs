using ProjectPSSC.Domain.Models;

namespace ProjectPSSC.Domain.Operations;

public class TransformOrderToInvoiceOperation
{
    public Invoice Execute(Order order)
    {
        var invoice = new Invoice(order.Id, order.TotalAmount);
        return invoice;
    }
}
