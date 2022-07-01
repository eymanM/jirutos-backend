using Foundation;
using Foundation.Interfaces;
using Foundation.Models;
using JiraService;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IssuesController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly IssueFascade _repo;
    private readonly ILogger<JiraIssueRepository> _logger;

    public IssuesController(IConfiguration config, IMapper mapper, ILogger<JiraIssueRepository> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _repo = new IssueFascade(new List<IIssueRepository>() { new JiraIssueRepository(logger, mapper) });
    }

    [HttpPost]
    public ActionResult DateRangeWorklogs([FromBody] DateRange scanDate)
    {
        var worklogs = _repo.WorklogsForDateRange(UserManagement.ResolveUser(), scanDate);
        return Ok(worklogs);
    }

    [HttpPost("{type}/{name}")]
    public ActionResult UpdateWorklog(string type, string name, [FromBody] UpdateWorklogModel model)
    {
        _repo.UpdateWorklog(UserManagement.ResolveUser(), model, type, name);
        return Ok();
    }
}