namespace Application.Common.Exceptions;

public class OperationFailedException : BaseException
{
    public OperationFailedException(string message) : base(message, 500)
    {
    }
}