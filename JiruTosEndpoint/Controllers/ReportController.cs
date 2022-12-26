using ClickUpService;
using Foundation.Interfaces;
using Foundation;
using JiraService;
using System.Xml.Linq;
using CsvHelper.Configuration;
using CsvHelper;
using System.Text;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace JiruTosEndpoint.Controllers;

[Authorize]
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
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        var listToCsv = _repo.ProjectsBasicReport(_db.FindUser(email));
        var totalHours = TimeSpan.FromMilliseconds(listToCsv.Sum(x => x.TotalTimeMS)).TotalHours;

        using var ms = new MemoryStream();
        using var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true));
        using (var cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
        {
            cw.WriteRecords(listToCsv);
            cw.NextRecord();
            cw.WriteField("");
            cw.WriteField("All spend hours");
            cw.WriteField(Math.Round(totalHours, 2).ToString());
        }

        return File(ms.ToArray(), "text/csv", $"projects_basic_report.csv");
    }

    [HttpGet()]
    public IActionResult IssuesBasicReport()
    {
        var email = User.Claims.ToList().First(x => x.Type == "cognito:username").Value;
        var listToCsv = _repo.IssuesBasicReport(_db.FindUser(email));
        var totalHours = TimeSpan.FromMilliseconds(listToCsv.Sum(x => x.TotalTimeMS)).TotalHours;

        using var ms = new MemoryStream();
        using var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true));
        using (var cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
        {
            cw.WriteRecords(listToCsv);
            cw.NextRecord();
            cw.WriteField("");
            cw.WriteField("");
            cw.WriteField("All spend hours");
            cw.WriteField(Math.Round(totalHours, 2).ToString());
        }

        return File(ms.ToArray(), "text/csv", $"issues_basic_report_{DateTime.UtcNow.ToShortDateString()}.csv");
    }
}

