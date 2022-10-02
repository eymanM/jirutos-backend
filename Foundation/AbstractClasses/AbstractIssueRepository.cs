using System.Net;

namespace Foundation.AbstractClasses;

public abstract class AbstractIssueRepository : IIssueRepository
{
    public AbstractIssueRepository() { }

    public abstract string Type { get; }

    public abstract IEnumerable<IssueWorklogDto> WorklogsForDateRange(Integration integration, DateRange dateRange);

    public abstract void UpdateWorklog(Integration integration, UpdateWorklogModel model);

    public abstract IEnumerable<IssueForFilter> FilterIssues(Integration integration, Filter filter);

    public abstract HttpStatusCode AddWorklog(Integration integration, AddWorklog worklogAddObj);
    public abstract bool IfIssueExist(Integration integration, string issueId);
}