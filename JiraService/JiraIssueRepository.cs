using System.Net;
using System.Reflection;

namespace JiraService;

public class JiraIssueRepository : AbstractIssueRepository
{
    private readonly ILogger<JiraIssueRepository> _logger;

    public override string Type => "Jira";

    public JiraIssueRepository(ILogger<JiraIssueRepository> logger, IMapper mapper) : base()
    {
        _logger = logger;
    }

    public override IEnumerable<IssueWorklogDto> WorklogsForDateRange(Integration integration, DateRange dates)
    {
        BodyJQLModel body = JQLQueryBuilder.CurrentUserDateBoundedWorklogs(dates);
        RestResponse response = RestClientRequestHandler.PostJQLResponse(integration, body);

        if (!response.IsSuccessful) throw new Exception(response.Content);

        IssuesReturnRootObj? issuesResponse = JsonConvert.DeserializeObject<IssuesReturnRootObj>(response.Content);

        if (issuesResponse is null || !issuesResponse.Issues.Any()) return new List<IssueWorklogDto>();

        return DtoBuilder.ToStandardWorklogModel(integration, issuesResponse, dates);
    }

    public override void UpdateWorklog(Integration integration, UpdateWorklogModel model)
    {
        RestResponse response = RestClientRequestHandler.UpdateWorklog(integration, model);
        if (!response.IsSuccessful) throw new Exception(response.Content);
    }

    public override List<IssueForFilter> FilterIssues(Integration integration, Filter filter)
    {
        List<IssueForFilter> res = new();
        string? projectJql = filter.Projects.Any() ? $"project in ({string.Join(",", filter.Projects)})" : "";
        string? statusJql = filter.Statuses.Any() ? $"status in ({string.Join(",", filter.Statuses)})" : "";

        string jql = (string.Join(" AND ", new string[] { projectJql, statusJql })).Trim();

        foreach (var item in filter.Others)
        {
            if (jql[^3..] != "AND") jql += "AND";
            jql += item switch
            {
                "Assigned to me" => " assignee = currentUser() ",
                "Created by me" => " creator = currentUser() ",
                "Watched by me" => " watcher = currentUser() ",
                _ => throw new NotImplementedException(),
            };
        }

        jql = jql.Trim();

        if (jql[^3..] == "AND") jql = jql[..^3];
        if (jql[..3] == "AND") jql = jql[4..];

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

    public override HttpStatusCode AddWorklog(Integration integration, AddWorklog worklogAddObj)
    {
        return RestClientRequestHandler.AddWorklog(integration, worklogAddObj);
    }

    public override bool IfIssueExist(Integration integration, string issueId)
    {
        RestResponse response = RestClientRequestHandler.IfIssueExist(integration, issueId);
        return response.IsSuccessful;
    }
}