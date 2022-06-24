using Foundation.Models;

namespace JiraService;

public class RestClientRequestHandler
{
    private readonly IConfiguration _config;
    private readonly RestClient _client;

    public static RestResponse GetJQLResponse(Integration integration, BodyJQLModel body, string path = @"/search")
    {
        RestRequest request = new(path);
        request.AddJsonBody(body);

        return getClient(integration.Settings).ExecutePostAsync(request).GetAwaiter().GetResult();
    }

    public RestResponse UpdateWorklog(UpdateWorklogModel model, string path = @"/issue/{issueId}/worklog/{id}")
    {
        RestRequest request = new(path);
        request.AddUrlSegment("issueId", model.IssueId);
        request.AddUrlSegment("id", model.Id);
        request.AddBody(model);

        return _client.ExecutePutAsync(request).GetAwaiter().GetResult();
    }

    private static RestClient getClient(Dictionary<string, string> settings)
    {
        return new(settings["URL"])
        {
            Authenticator = new HttpBasicAuthenticator(settings["Email"], settings["Token"])
        };
    }
}