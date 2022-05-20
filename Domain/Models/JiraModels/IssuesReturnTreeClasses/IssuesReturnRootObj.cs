namespace Domain.Models.JiraModels.IssuesReturnTreeClasses;

public class IssuesReturnRootObj
{
    [JsonProperty("startAt")]
    public int StartAt { get; set; }

    [JsonProperty("maxResults")]
    public int MaxResults { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("issues")]
    public Issue[] Issues { get; set; }
}