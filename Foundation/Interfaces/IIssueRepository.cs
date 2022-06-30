using Foundation.Models;

namespace Foundation.Interfaces;

public interface IIssueRepository
{
    string Type { get; }

    IEnumerable<IssueWorklog> WorklogsForDateRange(Integration integration, DateRange dateRange);

    void UpdateWorklog(Integration integration, UpdateWorklogModel model);
}