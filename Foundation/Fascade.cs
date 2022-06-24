using Foundation.Interfaces;
using Foundation.Models;

namespace Foundation;

public class Fascade
{
    private readonly List<IIssueRepository> _repositories;

    public Fascade(List<IIssueRepository> repositories)
    {
        _repositories = repositories;
    }

    public IEnumerable<IssueWorklogDto> WorklogsForDateRange(User user, DateRange dateRange)
    {
        List<IIssueRepository> userRepositories = new();
        List<IssueWorklogDto> allWorklogs = new();
        user.Integrations.ForEach(integration =>
        {
            List<IIssueRepository> repos = _repositories.FindAll(repo => repo.Type == integration.Type);
            repos.ForEach(repo => allWorklogs.AddRange(repo.WorklogsForDateRange(integration, dateRange)));
        });

        return allWorklogs;
    }
}