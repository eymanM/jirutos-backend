using JiraService;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IssuesController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public IssuesController(IConfiguration config, IMapper mapper)
    {
        _config = config;
        _mapper = mapper;
    }

    [HttpPost]
    public ActionResult DateRangeWorklogs([FromBody] ScanDateModel scanDate)
    {
        IWorklogRepo<WorklogForIssue> repo = new WorklogsRepoHandler(_config);
        var worklogs = (List<WorklogForIssue>)repo.WorklogsForDateRange(scanDate);

        List<WorklogForJiraIssueDto> dtos = new();
        foreach (var worklog in worklogs)
        {
            WorklogForJiraIssueDto dto = _mapper.Map<WorklogForJiraIssueDto>(worklog);
            dtos.Add(dto);
        }

        return Ok(dtos);
    }
}