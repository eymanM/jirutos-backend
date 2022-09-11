using ClickUpService;
using Foundation.Interfaces;
using Foundation;
using JiraService;
using System.Xml.Linq;
using CsvHelper.Configuration;
using CsvHelper;
using System.Text;
using System.Collections;

namespace JiruTosEndpoint.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ReportController : Controller
{
    private readonly ReportFascade _repo;
    private readonly IDatabase _db;

    public ReportController(IDatabase db)
    {
        _repo = new ReportFascade(new List<IReportRepository>() {
            new JiraReportRepository(), new ClickUpReportRepository() });
        _db = db;
    }

    [HttpGet()]
    public IActionResult ProjectsBasicReport()
    {
        var listToCsv = _repo.ProjectsBasicReport(_db.FindUser("ironoth12@gmail.com"));
        var cc = new CsvConfiguration(new System.Globalization.CultureInfo("pl-PL"));
        using var ms = new MemoryStream();
        using var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true));
        using (var cw = new CsvWriter(sw, cc)) cw.WriteRecords(listToCsv);

        return File(ms.ToArray(), "text/csv", $"projects_basic_report.csv");
    }

    [HttpGet()]
    public IActionResult IssuesBasicReport()
    {
        var listToCsv = _repo.IssuesBasicReport(_db.FindUser("ironoth12@gmail.com"));
        var cc = new CsvConfiguration(new System.Globalization.CultureInfo("pl-PL"));
        using var ms = new MemoryStream();
        using var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true));
        using (var cw = new CsvWriter(sw, cc)) cw.WriteRecords(listToCsv);

        return File(ms.ToArray(), "text/csv", $"issues_basic_report_{DateTime.UtcNow.ToShortDateString()}.csv");
    }
}

