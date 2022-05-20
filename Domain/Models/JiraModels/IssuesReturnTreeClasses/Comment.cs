namespace Domain.Models.JiraModels.IssuesReturnTreeClasses;

public class Comment
{
    [JsonProperty("content")]
    public CommentContentObject[] CommentContentObject { get; set; }
}