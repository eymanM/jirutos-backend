namespace Domain.Models.Dtos;

public class WorklogForJiraIssueDto
{
    public string Id { get; set; }
    public string Self { get; set; }
    public string IssueId { get; set; }
    public Comment Comment { get; set; }
    public DateTime StartedDT { get; set; }
    public string TimeSpent { get; set; }
}