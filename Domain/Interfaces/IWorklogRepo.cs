using Domain.Models.JiraModels;

namespace Domain.Interfaces;

public interface IWorklogRepo<Q>
{
    IEnumerable<Q> WorklogsForDateRange(ScanDateModel dates);
}