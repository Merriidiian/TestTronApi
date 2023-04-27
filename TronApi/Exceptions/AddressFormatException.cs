namespace TronApi.Exceptions;

public class AddressFormatException : Exception
{
    public AddressFormatException(string message) : base(message)
    {
    }
}