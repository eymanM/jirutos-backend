using Newtonsoft.Json;

namespace Domain.Models.JiraModels;

public class BodyJQLModel
{
    [JsonProperty("jql")]
    public string JQL { get; set; }

    [JsonProperty("fields")]
    public string[] Fields { get; set; }
}