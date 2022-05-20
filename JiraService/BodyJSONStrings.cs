namespace JiraService;

public class BodyJSONStrings
{
    public static BodyJQLModel CurrentUserDateBoundedWorklogs(ScanDateModel dates)
    {
        BodyJQLModel body = new()
        {
            JQL = $"worklogDate > {dates.DateFromDT:yyyy-MM-dd} and worklogDate < {dates.DateToDT:yyyy-MM-dd} " +
            "and worklogAuthor = currentUser()",
            Fields = new string[] { "worklog" }
        };

        return body;
    }
}