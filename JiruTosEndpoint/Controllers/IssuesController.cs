using JiraService;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IssuesController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly WorklogsRepoHandler _repo;

    public IssuesController(IConfiguration config, IMapper mapper)
    {
        _config = config;
        _mapper = mapper;
        _repo = new(config);
    }

    [HttpPost]
    public ActionResult DateRangeWorklogs([FromBody] ScanDateModel scanDate)
    {
        var worklogs = _repo.WorklogsForDateRange(scanDate).ToList();

        List<WorklogForJiraIssueDto> dtos = new();
        foreach (var worklog in worklogs)
        {
            WorklogForJiraIssueDto dto = _mapper.Map<WorklogForJiraIssueDto>(worklog);
            dtos.Add(dto);
        }

        return Ok(dtos);
    }

    [HttpPost]
    public ActionResult UpdateWorklog([FromBody] UpdateWorklogModel model)
    {
        _repo.UpdateWorklog(model);
        return Ok();
    }
}