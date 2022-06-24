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
    private readonly Fascade _repo;
    private readonly ILogger<JiraIssueRepository> _logger;

    public IssuesController(IConfiguration config, IMapper mapper, ILogger<JiraIssueRepository> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _repo = new Fascade(new List<IIssueRepository>() { new JiraIssueRepository(logger, mapper) });
    }

    private User resolveUser()
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            Integrations = new List<Integration>()
        };

        Integration integration = new()
        {
            Type = "Jira",
            Settings = new Dictionary<string, string>()
            {
              { "URL", @"https://psw-inzynierka.atlassian.net/rest/api/3" },
              { "Email", @"ironoth12@gmail.com" },
              { "Token", @"<token>" }
            }
        };

        user.Integrations.Add(integration);

        return user;
    }

    [HttpPost]
    public ActionResult DateRangeWorklogs([FromBody] DateRange scanDate)
    {
        var worklogs = _repo.WorklogsForDateRange(resolveUser(), scanDate).ToList();

        return Ok(worklogs);
    }

    [HttpPost]
    public ActionResult UpdateWorklog([FromBody] UpdateWorklogModel model)
    {
        //_repo.UpdateWorklog(model);
        return Ok();
    }
}