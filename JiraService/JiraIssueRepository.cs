using AutoMapper;
using Foundation.AbstractClasses;
using Foundation.Models;
using JiraService.JiraModels;
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

    public override IEnumerable<IssueWorklogDto> WorklogsForDateRange(Integration integration, DateRange dates)
    {
        BodyJQLModel body = JQLQueryBuilder.CurrentUserDateBoundedWorklogs(dates);
        RestResponse response = RestClientRequestHandler.GetJQLResponse(integration, body);

        if (!response.IsSuccessful) throw new Exception(response.Content);

        IssuesReturnRootObj? issuesResponse = JsonConvert.DeserializeObject<IssuesReturnRootObj>(response.Content);

        if (issuesResponse is null || !issuesResponse.Issues.Any()) return new List<IssueWorklogDto>();

        return _dtoBuilder.ToStandardWorklogModel(integration, issuesResponse, dates);
    }

    public override void UpdateWorklog(Integration integration, UpdateWorklogModel model)
    {
        RestResponse response = RestClientRequestHandler.UpdateWorklog(integration, model);
        if (!response.IsSuccessful) throw new Exception(response.Content);
    }

    public override List<IssueForFilter> FilterIssuesByJql(Integration integration, string jql)
    {
        List<IssueForFilter> res = new();
        BodyJQLModel body = JQLQueryBuilder.BodyFromString(jql, new string[] { "timetracking", "priority", "summary" });
        RestResponse response = RestClientRequestHandler.FilterIssuesByJql(integration, body);
        if (!response.IsSuccessful) throw new Exception(response.Content);
        var issues = JsonConvert.DeserializeObject<IssuesReturnRootObj>(response.Content)!.Issues;

        foreach (Issue item in issues)
        {
            res.Add(new()
            {
                IssueId = item.Id,
                Key = item.Key,
                Summary = item.Fields.Summary,
                TimeSpent = item.Fields.Timetracking.TimeSpent,
                Priority = item.Fields.Priority.Name,
                PriorityImage = item.Fields.Priority.IconUrl,
                Type = integration.Type,
                IntegrationName = integration.Name
            });
        }

        return res;
    }
}