using Foundation.Utils;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UtilsController : Controller
{

    [HttpGet("{ms::int}")]
    public ActionResult SpanStrFromMs(int ms) =>
        Ok(TimeSpanString.TSpanToSpanStr(TimeSpan.FromMilliseconds(ms)));
    
}