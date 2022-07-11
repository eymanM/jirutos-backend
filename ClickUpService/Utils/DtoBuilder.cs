namespace ClickUpService.Utils;

public class DtoBuilder
{
    private static readonly IMapper _mapper = GetMappers.ClickUpToStandardWorklog();

    public static List<IssueWorklogDto> ToStandardWorklogModel
        (Integration integration, List<ClickUpWorklog> logs, int teamId)
    {
        List<IssueWorklogDto> dtos = new();
        foreach (var worklog in logs)
        {
            IssueWorklogDto dto = _mapper.Map<IssueWorklogDto>(worklog);
            dto.Type = integration.Type;
            dto.IntegrationName = integration.Name;
            dto.CustomField1 = teamId.ToString();
            dtos.Add(dto);
        }

        return dtos;
    }
}