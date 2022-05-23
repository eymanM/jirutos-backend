namespace JiraService;

public class RestClientRequestHandler
{
    private readonly IConfiguration _config;
    private readonly RestClient _client;

    public RestClientRequestHandler(IConfiguration config)
    {
        _config = config;
        _client = new(_config["AppData:URL"])
        {
            Authenticator = new HttpBasicAuthenticator(_config["AppData:Email"], _config["AppData:Token"])
        };
    }

    public RestResponse GetJQLResponse(BodyJQLModel body, string path = @"/search")
    {
        RestRequest request = new(path);
        request.AddJsonBody(body);

        return _client.ExecutePostAsync(request).GetAwaiter().GetResult();
    }

    public RestResponse UpdateWorklog(UpdateWorklogModel model, string path = @"/issue/{issueId}/worklog/{id}")
    {
        RestRequest request = new(path);
        request.AddUrlSegment("issueId", model.IssueId);
        request.AddUrlSegment("id", model.Id);
        request.AddBody(model);

        return _client.ExecutePutAsync(request).GetAwaiter().GetResult();
    }
}