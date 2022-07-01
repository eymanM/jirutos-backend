using Foundation.Models;

namespace JiraService;

public class RestClientRequestHandler
{
    public static RestResponse GetJQLResponse(Integration integration, BodyJQLModel body, string path = @"/search")
    {
        RestRequest request = new(path);
        request.AddJsonBody(body);

        return getClient(integration.Settings).ExecutePostAsync(request).GetAwaiter().GetResult();
    }

    public static RestResponse UpdateWorklog(Integration integration, UpdateWorklogModel model, string path = @"/issue/{issueId}/worklog/{id}")
    {
        RestRequest request = new(path);
        request.AddUrlSegment("issueId", model.IssueId);
        request.AddUrlSegment("id", model.Id);
        request.AddBody(model);

        return getClient(integration.Settings).ExecutePutAsync(request).GetAwaiter().GetResult();
    }

    public static RestResponse AvailableProjectsForUser(Integration integration, string path = @"/permissions/project")
    {
        RestRequest request = new(path);
        List<string> permissionsForLogTime = new() { "WORK_ON_ISSUES" };
        request.AddBody(new { permissions = permissionsForLogTime });

        return getClient(integration.Settings).ExecutePostAsync(request).GetAwaiter().GetResult();
    }

    public static RestResponse AllStatuses(Integration integration, string path = @"/status")
    {
        RestRequest request = new(path);

        return getClient(integration.Settings).GetAsync(request).GetAwaiter().GetResult();
    }

    private static RestClient getClient(Dictionary<string, string> settings)
    {
        return new(settings["URL"])
        {
            Authenticator = new HttpBasicAuthenticator(settings["Email"], settings["Token"])
        };
    }
}