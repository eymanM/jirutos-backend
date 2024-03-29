﻿using JiraService;

namespace JiraService.Utils;

public class DtoBuilder
{
    private static readonly IMapper _mapper = GetMappers.JiraToStandardWorklog();

    public static List<IssueWorklogDto> ToStandardWorklogModel(Integration integration, IssuesReturnRootObj? root, DateRange dates)
    {
        List<WorklogForJiraIssue> worklogs = root.Issues
            .Select(x => x.Fields.Worklog.Worklogs
                    .Where(y => y.Author.EmailAddress == integration.Settings["Email"]) // TODO  filter in JQL with email
                    .Where(z => dates.DateFromDT <= z.startedDT && dates.DateToDT >= z.startedDT))
            .Aggregate((x, y) => x.Concat(y)) //combine worklogs from all issues to one list
        .ToList();

        List<IssueWorklogDto> dtos = new();
        foreach (var worklog in worklogs)
        {
            IssueWorklogDto dto = _mapper.Map<IssueWorklogDto>(worklog);
            dto.Type = integration.Type;
            dto.IntegrationName = integration.Name;
            dtos.Add(dto);
        }

        return dtos;
    }
}