using Microsoft.AspNetCore.Diagnostics;

namespace JiruTosEndpoint;

public class ErrorHandler
{
    private readonly ILogger _logger;

    public ErrorHandler(ILogger logger)
    {
        _logger = logger;
    }

    public (int, object?) HandleErrors(IExceptionHandlerPathFeature exFeature)
    {
        string message = exFeature.Error.Message ?? "Erorr";
        _logger.Log(LogLevel.Warning, message);

        //if (exFeature?.Error is StatusException)
        //{
        //    int statusCode = Convert.ToInt32(message);
        //    return (statusCode, getDefaultObjForStatusCode(statusCode));
        //}

        return
        (
            400,
            new
            {
                result = false,
                message = $"Error in {exFeature.Path} " +
                $"with message - " +
                $"{message.Replace("\r", "").Replace("\n", "")}"
            }
        );
    }

    //    private object? getDefaultObjForStatusCode(int statusCode) => statusCode switch
    //    {
    //        404 => new { },
    //        _ => null,
    //    };
}