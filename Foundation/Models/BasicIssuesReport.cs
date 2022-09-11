using CsvHelper.Configuration.Attributes;

namespace Foundation.Models;
public class BasicIssueReportModel
{
    public string Title { get; set; }

    public string Assignee { get; set; }

    [Name("Project Name")]
    public string ProjectName { get; set; }

    [Name("Total time")]
    public string TotalTime { get; set; }
}

