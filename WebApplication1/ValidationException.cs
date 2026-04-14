namespace WebApplication1;

public class ValidationException : Exception
{
    public int ErrorCode { get; private set; }
    
    public ValidationException(string message, int errorCode = -999) : base(message)
    {
        ErrorCode  = errorCode;
    }
}