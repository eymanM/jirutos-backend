using Foundation.Interfaces;
using Foundation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraService;

public class JiraDictionaryRepository : IDictionaryRepository
{
    public string Type => "Jira";

    public IEnumerable<Project> AvailableProjectsForUser(Integration integration)
    {
        var response = RestClientRequestHandler.AvailableProjectsForUser(integration);
        if (!response.IsSuccessful) throw new Exception(response.Content);

        ListOfProjects projects = JsonConvert.DeserializeObject<ListOfProjects>(response.Content);

        return projects.Projects;
    }

    public IEnumerable<Status> AllStatuses(Integration integration)
    {
        var response = RestClientRequestHandler.AllStatuses(integration);
        if (!response.IsSuccessful) throw new Exception(response.Content);

        List<Status> statuses = JsonConvert.DeserializeObject<List<Status>>(response.Content);

        return statuses;
    }
}