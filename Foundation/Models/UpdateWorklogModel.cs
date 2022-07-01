using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Foundation.Models.JiraModels;

public class UpdateWorklogModel
{
    public string Id { get; set; }

    public string IssueId { get; set; }

    [RegularExpression(@"^(?!.*([wdhml]).*\1)\d*\.?\d+[wdhml](?: \d*\.?\d+[wdhml])*$",
        ErrorMessage = "Time log is in wrong format")]
    public string TimeSpent { get; set; }

    public string Started { get; set; }
}