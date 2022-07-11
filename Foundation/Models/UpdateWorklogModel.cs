using System.ComponentModel.DataAnnotations;

namespace Foundation.Models;

public class UpdateWorklogModel
{
    public string Id { get; set; }

    public string IssueId { get; set; }

    [RegularExpression(@"^(?!.*([wdhml]).*\1)\d*\.?\d+[wdhml](?: \d*\.?\d+[wdhml])*$",
        ErrorMessage = "Time log is in wrong format")]
    public string TimeSpent { get; set; }

    public DateTime Started { get; set; }
    public string? CustomField1 { get; set; }
}