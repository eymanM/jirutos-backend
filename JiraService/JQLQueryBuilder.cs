using System.Runtime.InteropServices;

namespace JiraService;

public class JQLQueryBuilder
{
    public static BodyJQLModel CurrentUserDateBoundedWorklogs(DateRange dates)
    {
        BodyJQLModel body = new()
        {
            JQL = $"worklogDate > {dates.DateFromDT:yyyy-MM-dd} and worklogDate < {dates.DateToDT:yyyy-MM-dd} " +
            "and worklogAuthor = currentUser()",
            Fields = new string[] { "worklog" }
        };

        return body;
    }

    public static BodyJQLModel BodyFromString(string jql, [Optional] string[] fields)
    {
        BodyJQLModel body = new()
        {
            JQL = jql,
            Fields = fields ?? Array.Empty<string>()
        };

        return body;
    }
}