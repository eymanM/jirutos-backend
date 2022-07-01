using Foundation.Models;

namespace Foundation.Interfaces;

public interface IDictionaryRepository
{
    string Type { get; }

    IEnumerable<Project> AvailableProjectsForUser(Integration integration);

    IEnumerable<Status> AllStatuses(Integration integration);
}