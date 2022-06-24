namespace Foundation.Models;

public class User
{
    public Guid Id { get; set; }
    public List<Integration> Integrations { get; set; }

    public User()
    {
        Integrations = new();
    }
}