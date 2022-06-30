namespace Foundation.Models.Dtos;

public class IssueWorklog
{
    public string Id { get; set; }
    public string IssueId { get; set; }

    public string CommentText { get; set; }

    public DateTime StartedDT { get; set; }
    public string TimeSpent { get; set; }
    public string Type { get; set; }
    public string IntegrationName { get; set; }
}