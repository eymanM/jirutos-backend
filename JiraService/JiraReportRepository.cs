using Foundation.Utils;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace JiraService;

public class JiraReportRepository : IReportRepository
{
    public string Type => "Jira";

    public List<BasicIssueReportModel> IssuesBasicReport(Integration integration)
    {
        var projsResp = RestClientRequestHandler.AvailableProjectsForUser(integration);
        var def = SimpleHelpers.GetEmptyGenericList(new { id = "" });
        var projsIdsObjs = JsonConvert.DeserializeAnonymousType(projsResp.Content!, def)!;
        List<string> projIds = projsIdsObjs.Select(x => x.id).ToList();
        string jqlStr = $"projects in ({string.Join(",", projIds)})";

        BodyJQLModel body = JQLQueryBuilder.BodyFromString(jqlStr);
        RestResponse jqlResp = RestClientRequestHandler.GetJQLResponse(integration, body);
        var def2 = new
        {
            issues = SimpleHelpers.GetEmptyGenericList(new
            {
                fields = new { timespent = 0, project = new { key = "", name = "" },
                    assignee  = new { displayName = "Unassigned" }, summary = "" }
            })
        };
        var timeSpendObjs = JsonConvert.DeserializeAnonymousType(jqlResp.Content!, def2)!.issues;

        List<BasicIssueReportModel> resp = new();

        foreach (var item in timeSpendObjs)
            resp.Add(new BasicIssueReportModel
            {
                Title = item.fields.summary,
                Assignee = item.fields.assignee.displayName,
                ProjectName = item.fields.project.name,
                TotalWorkTime = TimeSpanString.TSpanToWorkSpanStr(TimeSpan.FromSeconds(item.fields.timespent)),
                TotalTimeMS = item.fields.timespent
            });

        return resp;
    }

    public List<BasicProjectReportModel> ProjectsBasicReport(Integration integration)
    {
        var projsResp = RestClientRequestHandler.AvailableProjectsForUser(integration);
        var def = SimpleHelpers.GetEmptyGenericList(new { id = "" });
        var projsIdsObjs = JsonConvert.DeserializeAnonymousType(projsResp.Content!, def)!;
        List<string> projIds = projsIdsObjs.Select(x => x.id).ToList();
        string jqlStr = $"projects in ({string.Join(",", projIds)})";

        BodyJQLModel body = JQLQueryBuilder.BodyFromString(jqlStr);
        RestResponse jqlResp = RestClientRequestHandler.GetJQLResponse(integration, body);
        var def2 = new { issues = SimpleHelpers.GetEmptyGenericList(new {
             fields = new { timespent = 0, project = new { key = "", name = "" } }} ) 
        };
        var timeSpendObjs = JsonConvert.DeserializeAnonymousType(jqlResp.Content!, def2)!.issues;
        var groupedByKey = timeSpendObjs.GroupBy(x => x.fields.project.key).ToList();

        List<BasicProjectReportModel> resp = new();
        foreach (var item in groupedByKey)
        {
            var items = item.ToList();
            var timeSecSum = items.Sum(x => x.fields.timespent);
            var ts = TimeSpan.FromSeconds(timeSecSum);
            resp.Add(new BasicProjectReportModel
            {
                Id = item.Key,
                Name = items.FirstOrDefault()!.fields.project.name,
                TotalWorkTime = TimeSpanString.TSpanToWorkSpanStr(ts),
                TotalTimeMS = ts.Milliseconds
            });
        }
        return resp;
    }
}
