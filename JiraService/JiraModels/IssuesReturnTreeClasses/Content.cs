namespace Foundation.Models.JiraModels.IssuesReturnTreeClasses;

public class CommentContentObject
{
    [JsonProperty("content")]
    public ContentExact[] Content { get; set; }
}