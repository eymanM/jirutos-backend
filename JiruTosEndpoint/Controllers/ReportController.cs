using ClickUpService;
using Foundation.Interfaces;
using Foundation;
using JiraService;
using System.Xml.Linq;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ReportController : Controller
{
    private readonly DictionaryFascade _repo;
    private readonly IDatabase _db;

    public ReportController(IDatabase db)
    {
        _repo = new DictionaryFascade(new List<IDictionaryRepository>() {
            new JiraDictionaryRepository(), new ClickUpDictionaryRepository() });
        _db = db;
    }
    [HttpGet("{type}/{name}")]
    public IActionResult Basic(string type, string name)
    {
        var projects = _repo.AvailableProjectsForUser(_db.FindUser("ironoth12@gmail.com"), type, name);
        return Ok(projects);
    }
}
