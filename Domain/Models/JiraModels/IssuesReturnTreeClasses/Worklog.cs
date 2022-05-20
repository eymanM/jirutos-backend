namespace Domain.Models.JiraModels.IssuesReturnTreeClasses;

public class Worklog
{
    [JsonProperty("startAt")]
    public int StartAt { get; set; }

    [JsonProperty("maxResults")]
    public int MaxResults { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("worklogs")]
    public WorklogForIssue[] Worklogs { get; set; }
}