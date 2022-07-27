

using Foundation.Interfaces;
using Foundation.Models.Structs;
using Status = Foundation.Models.Status;

namespace ClickUpService;

public class ClickUpDictionaryRepository : IDictionaryRepository
{
    public string Type => "ClickUp";

    public IEnumerable<Status> AllStatuses(Integration inte)
    {
        List<Status> statuses = new();
        List<RestResponse> resps = RestClientRequestHandler.AvailableProjectsForUser(inte);

        resps.ForEach(resp =>
        {
            var def = new { spaces = SimpleHelpers.GetEmptyGenericList(new { 
                statuses = SimpleHelpers.GetEmptyGenericList(new { status = "", id = "" } )})};

            var statusesData = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.spaces;
            statusesData.ForEach(s => statuses.AddRange(
                s.statuses.Select(x => new Status() { Id=x.status, Name=x.status}).ToList())
            );
        });

        return statuses.DistinctBy(p => p.Name);
    }

    public IEnumerable<Project> AvailableProjectsForUser(Integration inte)
    {
        List<Project> projs = new List<Project>();
        List<RestResponse> resps = RestClientRequestHandler.AvailableProjectsForUser(inte);

        resps.ForEach(resp =>
        {
            var def = new { spaces = new List<Project>() };
            var spacesData = JsonConvert.DeserializeAnonymousType(resp.Content!, def)!.spaces;
            projs.AddRange(spacesData);
        });

        return projs;
    }
}
