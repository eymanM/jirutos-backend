using Microsoft.AspNetCore.Diagnostics;

namespace JiruTosEndpoint.Controllers;

[Route("[controller]")]
[ApiController]
public class ErrorController : ControllerBase
{
    public IActionResult HandleErrors()
    {
        var exteptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        (int statusCode, object? returnObj) = ErrorService.ErrorService.HandleErrors(exteptionFeature);

        return StatusCode(statusCode, returnObj);
    }
}