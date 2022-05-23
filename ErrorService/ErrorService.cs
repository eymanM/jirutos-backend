using ErrorService.CustomExceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace ErrorService;

public static class ErrorService
{
    public static (int, object?) HandleErrors(IExceptionHandlerPathFeature exFeature)
    {
        if (exFeature?.Error is StatusCodeException)
        {
            int statusCode = Convert.ToInt32(exFeature.Error.Message);
            return (statusCode, getDefaultObjForStatusCode(statusCode));
        }

        return
        (
            400,
            new
            {
                result = false,
                message = $"Erorr in {exFeature.Path} " +
                $"with message - " +
                $"{exFeature.Error.Message.Replace("\r", "").Replace("\n", "")}"
            }
        );
    }

    private static object? getDefaultObjForStatusCode(int statusCode) => statusCode switch
    {
        404 => new { },
        _ => null,
    };
}