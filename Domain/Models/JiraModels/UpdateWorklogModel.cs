using System.ComponentModel.DataAnnotations;

namespace Domain.Models.JiraModels;

public class UpdateWorklogModel
{
    public int Id { get; set; }

    public int IssueId { get; set; }

    [RegularExpression(@"^(?!.*([wdhml]).*\1)\d*\.?\d+[wdhml](?: \d*\.?\d+[wdhml])*$",
        ErrorMessage = "Time log is in wrong format")]
    public string TimeSpent { get; set; }
}