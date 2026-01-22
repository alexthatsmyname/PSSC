namespace Billing.Api.Domain.Exceptions;

public class InvalidInvoiceException : Exception
{
    public InvalidInvoiceException(string message) : base(message)
    {
    }
}
