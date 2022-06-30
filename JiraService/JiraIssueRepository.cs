using AutoMapper;
using Foundation.AbstractClasses;
using Foundation.Models;
using JiraService.Utils;
using Microsoft.Extensions.Logging;

namespace JiraService;

public class JiraIssueRepository : AbstractIssueRepository
{
    private readonly ILogger<JiraIssueRepository> _logger;
    private readonly DtoBuilder _dtoBuilder;

    public override string Type => "Jira";

    public JiraIssueRepository(ILogger<JiraIssueRepository> logger, IMapper mapper) : base()
    {
        _logger = logger;
        _dtoBuilder = new(mapper);
    }

    public override IEnumerable<IssueWorklog> WorklogsForDateRange(Integration integration, DateRange dates)
    {
        BodyJQLModel body = JQLQueryBuilder.CurrentUserDateBoundedWorklogs(dates);
        RestResponse response = RestClientRequestHandler.GetJQLResponse(integration, body);

        if (!response.IsSuccessful) throw new Exception(response.Content);

        IssuesReturnRootObj? issuesResponse = JsonConvert.DeserializeObject<IssuesReturnRootObj>(response.Content);

        if (issuesResponse is null || !issuesResponse.Issues.Any()) return new List<IssueWorklog>();

        return _dtoBuilder.ToStandardWorklogModel(integration, issuesResponse, dates);
    }

    public override void UpdateWorklog(Integration integration, UpdateWorklogModel model)
    {
        RestResponse response = RestClientRequestHandler.UpdateWorklog(integration, model);
        if (!response.IsSuccessful) throw new Exception(response.Content);
    }
}