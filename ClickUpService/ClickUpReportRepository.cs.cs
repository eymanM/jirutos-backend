using Foundation.Interfaces;

namespace ClickUpService;

public class ClickUpReportRepository : IReportRepository
{
    public string Type => "ClickUp";

    public List<BasicIssueReportModel> IssuesBasicReport(Integration inte)
    {
        List<BasicIssueReportModel> objForCSV = new();
        List<Project> projs = new();
        List<RestResponse> projsResp = RestClientRequestHandler.AvailableProjectsForUser(inte);
        Dictionary<string, int> projTimeSpent = new();
        projsResp.ForEach(resp =>
        {
            var def = new { spaces = new List<Project>() };
            var spacesData = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.spaces;
            projs.AddRange(spacesData);
        });

        Filter filter = new() { Projects = projs.Select(x => x.Id).ToList() };
        var issuesResps = RestClientRequestHandler.FilterIssuesByJqlAsync(inte, filter);

        issuesResps.ForEach(resp =>
        {
            var def = new
            {
                tasks = SimpleHelpers.GetEmptyGenericList(new
                { name = "", space = new { id = "" }, time_spent = 0, 
                    assignees = SimpleHelpers.GetEmptyGenericList(new { username = "" }) })
            };

            var tasksData = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.tasks;
            objForCSV.AddRange(tasksData.Select(t => new BasicIssueReportModel
            {
                Title = t.name,
                Assignee = string.Join(',', t.assignees.Select(x => x.username)),
                ProjectName = projs.FirstOrDefault(p => p.Id == t.space.id)!.Name,
                TotalWorkTime = TimeSpanString.TSpanToWorkSpanStr(TimeSpan.FromMilliseconds(t.time_spent)),
                TotalTimeMS = t.time_spent,
            }
            ).ToList());
        });

        return objForCSV;
    }

    public List<BasicProjectReportModel> ProjectsBasicReport(Integration inte)
    {
        List<BasicProjectReportModel> objForCSV = new();
        List<Project> projs = new();
        List<RestResponse> projsResp = RestClientRequestHandler.AvailableProjectsForUser(inte);
        Dictionary<string, int> projTimeSpent = new();
        projsResp.ForEach(resp =>
        {
            var def = new { spaces = new List<Project>() };
            var spacesData = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.spaces;
            projs.AddRange(spacesData);
        });

        Filter filter = new() { Projects = projs.Select(x => x.Id).ToList() };
        var issuesResps = RestClientRequestHandler.FilterIssuesByJqlAsync(inte, filter);

        issuesResps.ForEach(resp =>
        {
            var def = new
            { 
                tasks = SimpleHelpers.GetEmptyGenericList(new { id = "", space = new { id = "" }, time_spent = 0 })
            };

            var tasksData = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.tasks;
            tasksData.ForEach(t => 
            {
                if(projTimeSpent.ContainsKey(t.space.id))
                   projTimeSpent[t.space.id] += t.time_spent;
                else projTimeSpent.Add(t.space.id, t.time_spent);
            });
        });

        foreach (var item in projTimeSpent)
            objForCSV.Add(new BasicProjectReportModel
            {
                Id = item.Key,
                Name = projs.FirstOrDefault(p => p.Id == item.Key)!.Name,
                TotalWorkTime = TimeSpanString.TSpanToWorkSpanStr(TimeSpan.FromMilliseconds(item.Value)),
                TotalTimeMS = item.Value
            });
        
        return objForCSV;
    }
}
