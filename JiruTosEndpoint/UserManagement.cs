using Foundation.Models;

namespace JiruTosEndpoint;

public class UserManagement
{
    public static User ResolveUser()
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            Integrations = new List<Integration>()
        };

        Integration integration = new()
        {
            Type = "Jira",
            Name = @"psw-inzynierka",
            Settings = new Dictionary<string, string>()
            {
              { "URL", @"https://psw-inzynierka.atlassian.net/rest/api/3" },
              { "Email", @"ironoth12@gmail.com" },
              { "Token", @"4gZKQ9EMC3Sel5XFhhAgF336" }
            },
        };

        //Integration integration2 = new()
        //{
        //    Type = "Jira",
        //    Name = @"psw-inzynierka2",
        //    Settings = new Dictionary<string, string>()
        //    {
        //      { "URL", @"https://psw-inzynierka2.atlassian.net/rest/api/3" },
        //      { "Email", @"stefanowicz20978@student.pswbp.pl" },
        //      { "Token", @"wZdCM7kFArrozquIZ05o30B7" }
        //    },
        //};

        user.Integrations.Add(integration);
        //user.Integrations.Add(integration2); ;

        return user;
    }
}