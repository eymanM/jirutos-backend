namespace Foundation;

public class DictionaryFascade
{
    private List<IDictionaryRepository> _repos { get; }

    public DictionaryFascade(List<IDictionaryRepository> repos)
    {
        _repos = repos;
    }

    public IEnumerable<Project> AvailableProjectsForUser(User user, string type, string name)
    {
        var integration = user.Integrations.FirstOrDefault(r => r.Type == type && r.Name == name);
        var repo = _repos.FirstOrDefault(r => r.Type == type);

        if (integration is null) throw new Exception("update worklog - integration not found");
        if (repo is null) throw new Exception("update worklog - repo not found");

        return repo.AvailableProjectsForUser(integration);
    }

    public IEnumerable<Status> AllStatuses(User user, string type, string name)
    {
        var integration = user.Integrations.FirstOrDefault(r => r.Type == type && r.Name == name);
        var repo = _repos.FirstOrDefault(r => r.Type == type);

        if (integration is null) throw new Exception("update worklog - integration not found");
        if (repo is null) throw new Exception("update worklog - repo not found");

        return repo.AllStatuses(integration);
    }
}