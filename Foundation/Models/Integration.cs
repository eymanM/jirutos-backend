namespace Foundation.Models;

public class Integration
{
    public string Type { get; set; }
    public Dictionary<string, string> Settings { get; set; }

    public Integration()
    {
        Settings = new();
    }
}