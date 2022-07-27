using System.Collections.Generic;

namespace ClickUpService;

public class ClickUpIssueRepository : AbstractIssueRepository
{
    public override string Type => "ClickUp";

    public override IEnumerable<IssueWorklogDto> WorklogsForDateRange(Integration integration, DateRange dateRange)
    {
        Dictionary<string, string> queryPar = new()
        {
            { "start_date",  new DateTimeOffset(dateRange.DateFromDT).ToUnixTimeMilliseconds().ToString()},
            { "end_date",  new DateTimeOffset(dateRange.DateToDT).ToUnixTimeMilliseconds().ToString()},
        };
        return RestClientRequestHandler.GetWorklogsForDateRange(integration, queryPar);
    }

    public override void UpdateWorklog(Integration integration, UpdateWorklogModel model)
    {
        RestResponse response = RestClientRequestHandler.UpdateWorklog(integration, model);
        if (!response.IsSuccessful) throw new Exception(response.Content);
    }

    public override IEnumerable<IssueForFilter> FilterIssues(Integration integration, Filter filter)
    {
        List<RestResponse> resps = RestClientRequestHandler.FilterIssuesByJqlAsync(integration, filter);
        List<IssueForFilter> issues = new();

        resps.ForEach(resp =>
        {
            var def = new {
            tasks = SimpleHelpers.GetEmptyGenericList(new
            {
                id = "", name = "", description = "",
                status = new { status = "" }, time_spent = 0,
            })};

            var tasksData = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.tasks;
            tasksData.ForEach(t => issues.Add(new IssueForFilter
            {
                IssueId = t.id,
                Key = t.name,
                Summary = t.description,
                TimeSpent = TimeSpanString.TSpanToSpanStr(TimeSpan.FromMilliseconds(t.time_spent)),
                Priority = t.status.status,
                Type = integration.Type,
                IntegrationName = integration.Name
            }));
        });

        return issues;
    }
}