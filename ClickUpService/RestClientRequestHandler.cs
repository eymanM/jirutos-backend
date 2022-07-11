using Foundation.Models.Structs;

namespace ClickUpService;

public class RestClientRequestHandler
{
    public static List<IssueWorklogDto> GetWorklogsForDateRange(Integration inte, Dictionary<string, string> queryParams)
    {
        List<IssueWorklogDto> worklogsRes = new();
        List<RestRequest> requests = new();

        var teamsIds = getUserTeamsId(inte.Settings);
        Func<IdStruct, RestRequest> addReqWithParams = x =>
        {
            RestRequest req = new("/team/" + x.Id + "/time_entries");
            foreach (var (key, value) in queryParams) req.AddQueryParameter(key, value);
            return req;
        };
        teamsIds.ForEach(team => requests.Add(addReqWithParams(team)));

        foreach (var (req, i) in requests.Select((req, i) => (req, i)))
        {
            RestResponse resp = getClient(inte.Settings).ExecuteGetAsync(req).GetAwaiter().GetResult();
            var def = new { data = new List<ClickUpWorklog>() };
            var worklogs = JsonConvert.DeserializeAnonymousType(resp.Content, def).data;
            worklogsRes.AddRange(DtoBuilder.ToStandardWorklogModel(inte, worklogs, teamsIds[i].Id));
        };

        return worklogsRes;
    }

    public static RestResponse UpdateWorklog(Integration integration, UpdateWorklogModel model)
    {
        RestRequest request = new("/team/{teamId}/time_entries/{id}");

        request.AddUrlSegment("teamId", model.CustomField1);
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

    private static RestClient getClient(Dictionary<string, string> settings)
    {
        RestClient client = new(settings["URL"]);
        client.AddDefaultHeader("Authorization", settings["Token"]);

        return client;
    }

    private static List<IdStruct> getUserTeamsId(Dictionary<string, string> settings)
    {
        RestRequest request = new("/team");
        var res = getClient(settings).ExecuteGetAsync(request).GetAwaiter().GetResult();
        var def = new { teams = new List<IdStruct>() };
        return JsonConvert.DeserializeAnonymousType(res.Content, def).teams;
    }
}