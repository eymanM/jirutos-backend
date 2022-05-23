namespace Domain.Models.Dtos;

public class WorklogForJiraIssueDto
{
    public string Id { get; set; }
    public string Self { get; set; }
    public string IssueId { get; set; }

    public string CommentText { get; set; }

    public DateTime StartedDT { get; set; }
    public string TimeSpent { get; set; }
}