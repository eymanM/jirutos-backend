using System.ComponentModel.DataAnnotations;

namespace Foundation.Models;

public class AddWorklog
{
    public string Id { get; set; }
    public string TimeSpend { get; set; }
    public long StartedUnix { get; set; }
    public string? CustomField { get; set; }
}
