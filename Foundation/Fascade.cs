﻿using Foundation.Interfaces;
using Foundation.Models;

namespace Foundation;

public class Fascade
{
    private readonly List<IIssueRepository> _repositories;

    public Fascade(List<IIssueRepository> repositories)
    {
        _repositories = repositories;
    }

    public IEnumerable<IssueWorklog> WorklogsForDateRange(User user, DateRange dateRange)
    {
        List<IIssueRepository> userRepositories = new();
        List<IssueWorklog> allWorklogs = new();
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
}