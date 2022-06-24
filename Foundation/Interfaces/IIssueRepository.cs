using Foundation.Models;

namespace Foundation.Interfaces;

public interface IIssueRepository
{
    string Type { get; }

    IEnumerable<IssueWorklogDto> WorklogsForDateRange(Integration integration, DateRange dateRange);
}