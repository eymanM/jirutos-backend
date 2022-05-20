namespace Domain.Models.JiraModels.IssuesReturnTreeClasses;

public class Updateauthor
{
    [JsonProperty("accountId")]
    public string AccountId { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set; }
}