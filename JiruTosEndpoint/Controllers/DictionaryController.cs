using Foundation;
using Foundation.Interfaces;
using JiraService;
using Microsoft.AspNetCore.Mvc;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DictionaryController : Controller
{
    private readonly DictionaryFascade _repo;

    public DictionaryController()
    {
        _repo = new DictionaryFascade(new List<IDictionaryRepository>() { new JiraDictionaryRepository() });
    }

    [HttpGet("{type}/{name}")]
    public ActionResult AvailableProjectsForUser(string type, string name)
    {
        var projects = _repo.AvailableProjectsForUser(UserManagement.ResolveUser(), type, name);
        return Ok(projects);
    }

    [HttpGet("{type}/{name}")]
    public ActionResult Statuses(string type, string name)
    {
        var projects = _repo.AllStatuses(UserManagement.ResolveUser(), type, name);
        return Ok(projects);
    }
}