using Foundation.Interfaces;
using Foundation.Models;
using System.Net;

namespace Foundation;

public class IssueFascade
{
    private readonly List<IIssueRepository> _repositories;

    public IssueFascade(List<IIssueRepository> repositories)
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

    public void UpdateWorklog(User user, UpdateWorklogModel model, string type, string name)
    {
        var integration = user.Integrations.FirstOrDefault(r => r.Type == type && r.Name == name);
        var repo = _repositories.FirstOrDefault(r => r.Type == type);

        if (integration is null) throw new Exception("update worklog - integration not found");
        if (repo is null) throw new Exception("update worklog - repo not found");

        repo.UpdateWorklog(integration, model);
    }

    public List<IssueForFilter> FilterIssuesByJql(User user, string type, string name, Filter filter)
    {
        var integration = user.Integrations.FirstOrDefault(r => r.Type == type && r.Name == name);
        var repo = _repositories.FirstOrDefault(r => r.Type == type);

        if (integration is null) throw new Exception("update worklog - integration not found");
        if (repo is null) throw new Exception("update worklog - repo not found");

        return repo.FilterIssues(integration, filter).ToList();
    }

    public HttpStatusCode AddWorklog(User user, string type, string name, AddWorklog worklogAddObj)
    {
        var integration = user.Integrations.FirstOrDefault(r => r.Type == type && r.Name == name);
        var repo = _repositories.FirstOrDefault(r => r.Type == type);

        if (integration is null) throw new Exception("update worklog - integration not found");
        if (repo is null) throw new Exception("update worklog - repo not found");

        return repo.AddWorklog(integration, worklogAddObj);
    }

    public Object IsIssueExist(User user, string type, string name, string issueId)
    {
        var integration = user.Integrations.FirstOrDefault(r => r.Type == type && r.Name == name);
        var repo = _repositories.FirstOrDefault(r => r.Type == type);

        if (integration is null) throw new Exception("update worklog - integration not found");
        if (repo is null) throw new Exception("update worklog - repo not found");

        return repo.IfIssueExist(integration, issueId);
    }
}