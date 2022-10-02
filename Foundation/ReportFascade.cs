using Foundation.Models;
using System.Collections.Generic;

namespace Foundation;

public class ReportFascade
{
    private List<IReportRepository> _repos { get; }

    public ReportFascade(List<IReportRepository> repos)
    {
        _repos = repos;
    }

    public List<BasicProjectReportModel> ProjectsBasicReport(User user)
    {
        List<BasicProjectReportModel> combinedData = new();
        user.Integrations.ForEach(integration =>
        {
            List<IReportRepository> repos = _repos.FindAll(repo => repo.Type == integration.Type);
            repos.ForEach(repo => combinedData.AddRange(repo.ProjectsBasicReport(integration)));
        });
        return combinedData;
    }

    public List<BasicIssueReportModel> IssuesBasicReport(User user)
    {
        List<BasicIssueReportModel> combinedData = new();
        user.Integrations.ForEach(integration =>
        {
            List<IReportRepository> repos = _repos.FindAll(repo => repo.Type == integration.Type);
            repos.ForEach(repo => combinedData.AddRange(repo.IssuesBasicReport(integration)));
        });
        return combinedData;
    }

}
