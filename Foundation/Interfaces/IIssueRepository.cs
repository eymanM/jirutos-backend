using Foundation.Models;

namespace Foundation.Interfaces;

public interface IIssueRepository
{
    string Type { get; }

    IEnumerable<IssueWorklogDto> WorklogsForDateRange(Integration integration, DateRange dateRange);

    void UpdateWorklog(Integration integration, UpdateWorklogModel model);

    IEnumerable<IssueForFilter> FilterIssuesByJql(Integration integration, string jql);
}