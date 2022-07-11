namespace JiraService;

public class GetMappers
{
    public static IMapper JiraToStandardWorklog()
    {
        return new MapperConfiguration(cfg => cfg.CreateMap<WorklogForJiraIssue, IssueWorklogDto>()
        .ForMember(x => x.CommentText, y => y
           .MapFrom(z => z.Comment.CommentContentObject
                  .FirstOrDefault().Content.FirstOrDefault().Text)))
        .CreateMapper();
    }
}