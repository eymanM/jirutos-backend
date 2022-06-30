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
            Name = @"psw-inzynierka",
            Settings = new Dictionary<string, string>()
            {
              { "URL", @"https://psw-inzynierka.atlassian.net/rest/api/3" },
              { "Email", @"ironoth12@gmail.com" },
              { "Token", @"T1J07OmkCFC3hDUQ0FHS9F2C" }
            },
        };

        Integration integration2 = new()
        {
            Type = "Jira",
            Name = @"psw-inzynierka2",
            Settings = new Dictionary<string, string>()
            {
              { "URL", @"https://psw-inzynierka2.atlassian.net/rest/api/3" },
              { "Email", @"stefanowicz20978@student.pswbp.pl" },
              { "Token", @"wZdCM7kFArrozquIZ05o30B7" }
            },
        };

        user.Integrations.Add(integration);
        user.Integrations.Add(integration2); ;

        return user;
    }

    [HttpPost]
    public ActionResult DateRangeWorklogs([FromBody] DateRange scanDate)
    {
        var worklogs = _repo.WorklogsForDateRange(resolveUser(), scanDate);
        return Ok(worklogs);
    }

    [HttpPost("{type}/{name}")]
    public ActionResult UpdateWorklog(string type, string name, [FromBody] UpdateWorklogModel model)
    {
        _repo.UpdateWorklog(resolveUser(), model, type, name);
        return Ok();
    }
}