using Foundation.Interfaces;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : Controller
{
    private readonly IDatabase _db;

    public UserController(IDatabase db)
    {
        _db = db;
    }

    [HttpGet("{email}")]
    public ActionResult Integrations(string email)
    {
        var user = _db.FindUser(email);
        var integrations = user.Integrations;
        var basicIntegrationsData = integrations.Select(integ => new { type = integ.Type, name = integ.Name });
        return Ok(basicIntegrationsData);
    }
}
