namespace TronApi.Exceptions;

public class TransactionNullException : Exception
{
    public TransactionNullException(string message) : base(message)
    {
    }
}