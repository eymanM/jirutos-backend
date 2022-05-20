namespace Domain.Models.JiraModels.IssuesReturnTreeClasses;

public class Fields
{
    [JsonProperty("worklog")]
    public Worklog Worklog { get; set; }
}