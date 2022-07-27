using ClickUpService;
using Foundation;
using Foundation.Interfaces;
using JiraService;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DictionaryController : Controller
{
    private readonly DictionaryFascade _repo;
    private readonly IDatabase _db;

    public DictionaryController(IDatabase db)
    {
        _repo = new DictionaryFascade(new List<IDictionaryRepository>() { 
            new JiraDictionaryRepository(), new ClickUpDictionaryRepository() });
        _db = db;
    }

    [HttpGet("{type}/{name}")]
    public ActionResult AvailableProjectsForUser(string type, string name)
    {
        var projects = _repo.AvailableProjectsForUser(_db.FindUser("ironoth12@gmail.com"), type, name);
        return Ok(projects);
    }

    [HttpGet("{type}/{name}")]
    public ActionResult Statuses(string type, string name)
    {
        var statuses = _repo.AllStatuses(_db.FindUser("ironoth12@gmail.com"), type, name);
        return Ok(statuses);
    }
}