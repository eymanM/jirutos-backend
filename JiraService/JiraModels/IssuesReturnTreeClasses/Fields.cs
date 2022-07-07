using JiraService.JiraModels.IssuesReturnTreeClasses;

namespace Foundation.Models.JiraModels.IssuesReturnTreeClasses;

public class Fields
{
    public Worklog Worklog;
    public string Summary { get; set; }
    public Priority Priority { get; set; }

    [JsonProperty("timetracking")]
    public Timetracking Timetracking { get; set; }
}