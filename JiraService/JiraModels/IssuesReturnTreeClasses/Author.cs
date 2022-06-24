namespace Foundation.Models.JiraModels.IssuesReturnTreeClasses;

public class Author
{
    [JsonProperty("accountId")]
    public string AccountId { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set; }
}