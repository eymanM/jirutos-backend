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
    private readonly IDatabase _db;
    private readonly IMapper _mapper;
    private readonly IssueFascade _repo;
    private readonly ILogger<JiraIssueRepository> _logger;

    public IssuesController(IDatabase db, IMapper mapper, ILogger<JiraIssueRepository> logger)
    {
        _db = db;
        _mapper = mapper;
        _logger = logger;
        _repo = new IssueFascade(new List<IIssueRepository>() { new JiraIssueRepository(logger, mapper) });
    }

    [HttpPost]
    public ActionResult DateRangeWorklogs([FromBody] DateRange scanDate)
    {
        var worklogs = _repo.WorklogsForDateRange(_db.FindUser("ironoth12@gmail.com"), scanDate);
        return Ok(worklogs);
    }

    [HttpPost("{type}/{name}")]
    public ActionResult UpdateWorklog(string type, string name, [FromBody] UpdateWorklogModel model)
    {
        _repo.UpdateWorklog(_db.FindUser("ironoth12@gmail.com"), model, type, name);
        return Ok();
    }
}