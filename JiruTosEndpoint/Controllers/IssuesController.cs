using ClickUpService;
using Foundation;
using Foundation.Interfaces;
using Foundation.Models;
using JiraService;
using Microsoft.AspNetCore.Authorization;

namespace JiruTosEndpoint.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class IssuesController : ControllerBase
{
    private readonly IDatabase _db;
    private readonly IssueFascade _repo;

    public IssuesController(IDatabase db, IMapper mapper, ILogger<JiraIssueRepository> logger)
    {
        _db = db;
        _repo = new IssueFascade(new List<IIssueRepository>() {
            new JiraIssueRepository(logger, mapper), new ClickUpIssueRepository()
        });
    }

    [HttpPost]
    public ActionResult DateRangeWorklogs([FromBody] DateRange scanDate)
    {
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        var worklogs = _repo.WorklogsForDateRange(_db.FindUser(email), scanDate);
        return Ok(worklogs);
    }

    [HttpPost("{type}/{name}")]
    public ActionResult UpdateWorklog(string type, string name, [FromBody] UpdateWorklogModel model)
    {
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        _repo.UpdateWorklog(_db.FindUser(email), model, type, name);
        return Ok();
    }

    [HttpPost("{type}/{name}")]
    public ActionResult FilterIssues(string type, string name, [FromBody] Filter filter)
    {
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        var issues = _repo.FilterIssuesByJql(_db.FindUser(email), type, name, filter);
        return Ok(issues);
    }

    [HttpPost("{type}/{name}")]
    public ActionResult AddWorklog(string type, string name, [FromBody] AddWorklog addWorklogObj)
    {
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        var status = _repo.AddWorklog(_db.FindUser(email), type, name, addWorklogObj);
        return Ok(status);
    }

    [HttpGet("{type}/{name}/{issueId}")]
    public ActionResult IfIssueExist(string type, string name, string issueId)
    {
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        var resp = _repo.IsIssueExist(_db.FindUser(email), type, name, issueId);
        return Ok(new { Exist = resp});
    }
}
