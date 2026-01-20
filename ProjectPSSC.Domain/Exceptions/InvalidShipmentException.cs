namespace ProjectPSSC.Domain.Exceptions;

public class InvalidShipmentException : Exception
{
    public InvalidShipmentException(string message) : base(message)
    {
    }
}
