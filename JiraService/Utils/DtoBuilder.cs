using AutoMapper;

namespace JiraService.Utils;

public class DtoBuilder
{
    private readonly IMapper _mapper;

    public DtoBuilder(IMapper mapper)
    {
        _mapper = mapper;
    }

    public List<IssueWorklogDto> ToStandartWorklogModel(string email, IssuesReturnRootObj? root, DateRange dates)
    {
        List<WorklogForJiraIssue> worklogs = root.Issues
            .Select(x => x.Fields.Worklog.Worklogs
                    .Where(y => y.Author.EmailAddress == email)
                    .Where(z => dates.DateFromDT <= z.startedDT && dates.DateToDT >= z.startedDT))
            .Aggregate((x, y) => x.Concat(y)) //combine worklogs from all issues to one list
        .ToList();

        List<IssueWorklogDto> dtos = new();
        foreach (var worklog in worklogs)
        {
            IssueWorklogDto dto = _mapper.Map<IssueWorklogDto>(worklog);
            dto.Type = "Jira";
            dtos.Add(dto);
        }

        return dtos;
    }
}