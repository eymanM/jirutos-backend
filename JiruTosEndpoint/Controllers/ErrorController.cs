using Microsoft.AspNetCore.Diagnostics;

namespace JiruTosEndpoint.Controllers;

[Route("[controller]")]
[ApiController]
public class ErrorController : ControllerBase
{
    private readonly ErrorHandler eService;

    public ErrorController(ILogger logger)
    {
        eService = new(logger);
    }

    public IActionResult HandleErrors()
    {
        var exteptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        (int statusCode, object? returnObj) = eService.HandleErrors(exteptionFeature);

        return StatusCode(statusCode, returnObj);
    }
}