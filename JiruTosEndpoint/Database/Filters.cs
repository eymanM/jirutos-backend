using Foundation.Models;
using MongoDB.Driver;

namespace JiruTosEndpoint.Database;

public class Filters
{
    public static FilterDefinition<User> GetFindUserByMailFilter(string email)
    {
        var builder = Builders<User>.Filter;
        return builder.Eq("Email", email);
    }
}