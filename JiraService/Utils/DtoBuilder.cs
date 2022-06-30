using AutoMapper;
using Foundation.Models;

namespace JiraService.Utils;

public class DtoBuilder
{
    private readonly IMapper _mapper;

    public DtoBuilder(IMapper mapper)
    {
        _mapper = mapper;
    }

    public List<IssueWorklog> ToStandardWorklogModel(Integration integration, IssuesReturnRootObj? root, DateRange dates)
    {
        List<WorklogForJiraIssue> worklogs = root.Issues
            .Select(x => x.Fields.Worklog.Worklogs
                    .Where(y => y.Author.EmailAddress == integration.Settings["Email"]) // TODO  filter in JQL with email
                    .Where(z => dates.DateFromDT <= z.startedDT && dates.DateToDT >= z.startedDT))
            .Aggregate((x, y) => x.Concat(y)) //combine worklogs from all issues to one list
        .ToList();

        List<IssueWorklog> dtos = new();
        foreach (var worklog in worklogs)
        {
            IssueWorklog dto = _mapper.Map<IssueWorklog>(worklog);
            dto.Type = integration.Type;
            dto.IntegrationName = integration.Name;
            dtos.Add(dto);
        }

        return dtos;
    }
}