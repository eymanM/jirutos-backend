using Foundation.Models.Structs;

namespace ClickUpService;

public class RestClientRequestHandler
{
    public static List<IssueWorklogDto> GetWorklogsForDateRange(Integration inte, Dictionary<string, string> queryParams)
    {
        List<IssueWorklogDto> worklogsRes = new();
        List<RestRequest> requests = new();

        requests = reqsForTeams(inte, "/team/{teamId}/time_entries").Select(req =>
        {
            foreach (var (key, value) in queryParams) req.AddQueryParameter(key, value);
            return req;
        }).ToList();

        foreach (var (req, i) in requests.Select((req, i) => (req, i)))
        {
            RestResponse resp = getClient(inte.Settings).ExecuteGetAsync(req).GetAwaiter().GetResult();
            var def = new { data = new List<ClickUpWorklog>() };
            var worklogs = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.data;

            worklogsRes.AddRange(DtoBuilder.ToStandardWorklogModel(inte, worklogs,
                req.Parameters.ToList().FirstOrDefault().Value.ToString()));
        };

        return worklogsRes;
    }

    public static RestResponse UpdateWorklog(Integration integration, UpdateWorklogModel model)
    {
        RestRequest request = new("/team/{teamId}/time_entries/{id}");

        request.AddUrlSegment("teamId", model.CustomField1!);
        request.AddUrlSegment("id", model.Id);
        long duration = (long)TimeSpanString.SpanStrToTSpan(model.TimeSpent).TotalMilliseconds;
        long start = ((DateTimeOffset)model.Started).ToUnixTimeMilliseconds();
        var def = new
        {
            start = start.ToString(),
            end = (start + duration).ToString(),
            duaration = duration.ToString(),
        };
        request.AddBody(def);

        return getClient(integration.Settings).ExecutePutAsync(request).GetAwaiter().GetResult();
    }
    public static List<RestResponse> FilterIssuesByJqlAsync(Integration inte, Filter filter)
    {
        var reqs = reqsForTeams(inte, "/team/{teamId}/task");

        filter.Projects.ForEach(f => reqs.ForEach(r => r.AddQueryParameter("space_ids[]", f)));
        filter.Statuses.ForEach(s => reqs.ForEach(r => r.AddQueryParameter("statuses[]", s)));
        var user = getCurrentUserData(inte);

        foreach (string item in filter.Others)
        {
            var param = item switch
            {
                "Assigned to me" => "assignees[]",
                "Created by me" => "created[]",
                "Watched by me" => "watched[]",
                _ => throw new NotImplementedException(),
            };
            reqs.ForEach(r => r.AddQueryParameter(param, user.Id));
        }
        reqs.ForEach(r => r.AddQueryParameter("include_closed", true));
        return reqs.Select(r => getClient(inte.Settings).ExecuteGetAsync(r).GetAwaiter().GetResult()).ToList();
    }

    public static List<RestResponse> AvailableProjectsForUser(Integration inte, string path = @"/team/{teamId}/space")
    {
        var reqs = reqsForTeams(inte, path);
        var resps = reqs.Select(req => getClient(inte.Settings).ExecuteGetAsync(req).GetAwaiter().GetResult()).ToList();
        return resps;
    }

    public static List<RestResponse> AllStatuses(Integration inte, string path = @"/team/{teamId}/space")
    {
        var reqs = reqsForTeams(inte, path);
        var resps = reqs.Select(req => getClient(inte.Settings).ExecuteGetAsync(req).GetAwaiter().GetResult()).ToList();
        return resps;
    }

    private static IdNameStruct getCurrentUserData(Integration inte, string path = @"/user")
    {
        RestRequest req = new(path);
        var resp = getClient(inte.Settings).ExecuteGetAsync(req).GetAwaiter().GetResult();
        var def = new { user = new { id = 0, username = "" } };
        var obj = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.user;
        return new IdNameStruct { Id = obj.id, Name = obj.username };
    }

    private static List<RestRequest> reqsForTeams(Integration inte, string path)
    {
        List<RestRequest> requests = new();
        var teamsIds = getUserTeamsId(inte.Settings);

        RestRequest getReqForTeam(IdNameStruct team) => (new RestRequest(path)).AddUrlSegment("teamId", team.Id);

        foreach (var team in teamsIds) requests.Add(getReqForTeam(team));

        return requests;
    }

    private static RestClient getClient(Dictionary<string, string> settings)
    {
        RestClient client = new(settings["URL"]);
        client.AddDefaultHeader("Authorization", settings["Token"]);

        return client;
    }

    private static List<IdNameStruct> getUserTeamsId(Dictionary<string, string> settings)
    {
        RestRequest request = new("/team");
        var res = getClient(settings).ExecuteGetAsync(request).GetAwaiter().GetResult();
        var def = new { teams = new List<IdNameStruct>() };
        var teams =  JsonConvert.DeserializeAnonymousType(res.Content!, def)!.teams;
        return teams;
    }
}