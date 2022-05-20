namespace JiraService;

public class WorklogsRepoHandler : WorklogRepoAbstract, IWorklogRepo<WorklogForIssue>
{
    public WorklogsRepoHandler(IConfiguration config) : base(config)
    {
    }

    public IEnumerable<WorklogForIssue> WorklogsForDateRange(ScanDateModel dates)
    {
        var requestHandler = new RestClientRequestHandler(Config);
        RestResponse response = requestHandler.GetJQLResponse(BodyJSONStrings.CurrentUserDateBoundedWorklogs(dates));

        if (!response.IsSuccessful) throw new Exception("404");

        IssuesReturnRootObj? issuesResponse = JsonConvert.DeserializeObject<IssuesReturnRootObj>(response.Content);

        if (issuesResponse is null) throw new Exception("202");

        List<WorklogForIssue> worklogs = issuesResponse.Issues
            .Select(x => x.Fields.Worklog.Worklogs
                    .Where(y => y.Author.EmailAddress == Config["AppData:Email"])
                    .Where(z => dates.DateFromDT <= z.startedDT && dates.DateToDT >= z.startedDT))
            .Aggregate((x, y) => x.Concat(y)) //combine worklogs from all issues to one list
            .ToList();

        return worklogs;
    }
}