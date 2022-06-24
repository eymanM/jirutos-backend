namespace Foundation.Models.JiraModels.IssuesReturnTreeClasses;

public class Issue
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("self")]
    public string Self { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("fields")]
    public Fields Fields { get; set; }
}