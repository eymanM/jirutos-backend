namespace ClickUpService;

public class GetMappers
{
    public static IMapper ClickUpToStandardWorklog()
    {
        return new MapperConfiguration(cfg => cfg.CreateMap<ClickUpWorklog, IssueWorklogDto>()
        .ForMember(a => a.Id, b => b.MapFrom(c => c.Id))
        .ForMember(a => a.IssueId, b => b.MapFrom(c => c.Task.Id))
        .ForMember(a => a.CommentText, b => b.MapFrom(c => c.Description))
        .ForMember(a => a.TimeSpent, b => b.MapFrom(c => TimeSpanString.TSpanToSpanStr(TimeSpan.FromMilliseconds(c.Duration))))
        .ForMember(a => a.StartedDT, b => b.MapFrom(c => SimpleUtils.UnixTimeToDT(c.Start))))
        .CreateMapper();
    }
}