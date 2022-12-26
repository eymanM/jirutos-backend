using CsvHelper.Configuration.Attributes;

namespace Foundation.Models;

public class BasicProjectReportModel
{
    public string Id { get; set; }
    public string Name { get; set; }

    [Name("Total time")]
    public string TotalWorkTime { get; set; }

    [Ignore]
    public int TotalTimeMS { get; set; }
}
