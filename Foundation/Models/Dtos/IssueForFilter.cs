namespace Foundation.Models.Dtos;

public class IssueForFilter
{
    public string IssueId { get; set; }
    public string Key { get; set; }
    public string Summary { get; set; }
    public string TimeSpent { get; set; }
    public string Priority { get; set; }
    public string PriorityImage { get; set; }

    public string Type { get; set; }
    public string IntegrationName { get; set; }
}