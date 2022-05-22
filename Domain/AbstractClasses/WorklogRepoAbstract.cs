namespace Domain.AbstractClasses;

public class WorklogRepoAbstract
{
    public readonly IConfiguration Config;

    public WorklogRepoAbstract(IConfiguration config)
    {
        Config = config;
    }
}