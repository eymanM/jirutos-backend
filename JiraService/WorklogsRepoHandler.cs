using ErrorService.CustomExceptions;

namespace JiraService;

public class WorklogsRepoHandler : WorklogRepoAbstract
{
    private readonly RestClientRequestHandler _requestHandler;

    public WorklogsRepoHandler(IConfiguration config) : base(config)
    {
        _requestHandler = new(config);
    }

    public IEnumerable<WorklogForJiraIssue> WorklogsForDateRange(ScanDateModel dates)
    {
        RestResponse response = _requestHandler.GetJQLResponse(BodyJSONStrings.CurrentUserDateBoundedWorklogs(dates));

        if (!response.IsSuccessful) throw new Exception(response.Content);

        IssuesReturnRootObj? issuesResponse = JsonConvert.DeserializeObject<IssuesReturnRootObj>(response.Content);

        if (issuesResponse is null) throw new StatusCodeException(404);

        List<WorklogForJiraIssue> worklogs = issuesResponse.Issues
            .Select(x => x.Fields.Worklog.Worklogs
                    .Where(y => y.Author.EmailAddress == Config["AppData:Email"])
                    .Where(z => dates.DateFromDT <= z.startedDT && dates.DateToDT >= z.startedDT))
            .Aggregate((x, y) => x.Concat(y)) //combine worklogs from all issues to one list
            .ToList();

        return worklogs;
    }

    public void UpdateWorklog(UpdateWorklogModel model)
    {
        var resp = _requestHandler.UpdateWorklog(model);
        if (resp.IsSuccessful is false) 
            throw new Exception(String.IsNullOrEmpty(resp.Content) ? resp.ErrorException.Message : resp.Content);
    }
}