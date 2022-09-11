namespace Foundation.Interfaces;

public interface IReportRepository
{
    string Type { get; }

    List<BasicProjectReportModel> ProjectsBasicReport(Integration integration);

    List<BasicIssueReportModel> IssuesBasicReport(Integration integration);
}
