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

    public override IEnumerable<IssueForFilter> FilterIssuesByJql(Integration integration, string jql)
    {
        throw new NotImplementedException();
    }
}