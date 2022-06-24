namespace Foundation.Models.JiraModels.IssuesReturnTreeClasses;

public class WorklogForJiraIssue
{
    [JsonProperty("self")]
    public string Self { get; set; }

    [JsonProperty("author")]
    public Author Author { get; set; }

    [JsonProperty("updateAuthor")]
    public Updateauthor UpdateAuthor { get; set; }

    [JsonProperty("comment")]
    public Comment Comment { get; set; }

    public DateTime CreatedDT { get; set; }

    private string _created;

    [JsonProperty("created")]
    public string Created
    {
        get { return _created; }
        set
        {
            _created = value;
            CreatedDT = DateTime.Parse(value);
        }
    }

    public DateTime UpdatedDT { get; set; }

    private string _updated;

    [JsonProperty("updated")]
    public string Updated
    {
        get { return _updated; }
        set
        {
            _updated = value;
            UpdatedDT = DateTime.Parse(value);
        }
    }

    public DateTime startedDT { get; set; }

    private string _started;

    [JsonProperty("started")]
    public string Started
    {
        get { return _started; }
        set
        {
            _started = value;
            startedDT = DateTime.Parse(value);
        }
    }

    [JsonProperty("timeSpent")]
    public string TimeSpent { get; set; }

    [JsonProperty("timeSpentSeconds")]
    public int TimeSpentSeconds { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("issueId")]
    public string IssueId { get; set; }
}