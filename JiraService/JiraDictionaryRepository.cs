namespace JiraService;

public class JiraDictionaryRepository : IDictionaryRepository
{
    public string Type => "Jira";

    public IEnumerable<Project> AvailableProjectsForUser(Integration integration)
    {
        List<Project> projects = new List<Project>();
        var response = RestClientRequestHandler.AvailableProjectsForUser(integration);
        if (!response.IsSuccessful) throw new Exception(response.Content);

        dynamic[] projectsJObject = JsonConvert.DeserializeObject<dynamic[]>(response.Content)!;
        foreach (var item in projectsJObject)
        {
            projects.Add(new Project { Id = item.id, Key = item.key, Name = item.name });
        }

        return projects;
    }

    public IEnumerable<Status> AllStatuses(Integration integration)
    {
        var response = RestClientRequestHandler.AllStatuses(integration);
        if (!response.IsSuccessful) throw new Exception(response.Content);

        List<Status> statuses = JsonConvert.DeserializeObject<List<Status>>(response.Content);

        return statuses;
    }
}