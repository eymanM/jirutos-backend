namespace ErrorService.CustomExceptions;

public class StatusCodeException : Exception
{
    public StatusCodeException(int statusCode) : base(statusCode.ToString())
    { }
}