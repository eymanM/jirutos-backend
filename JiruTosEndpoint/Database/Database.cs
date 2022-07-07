using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Foundation.Models;
using Foundation.Interfaces;

namespace JiruTosEndpoint.Database;

public class Database : IDatabase
{
    private readonly MongoClient client;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<User> userCollection;

    public Database(string connStr)
    {
        client = new MongoClient(connStr);
        database = client.GetDatabase("Jirutos");
        userCollection = database.GetCollection<User>("User");
    }

    public void InsertOrReplaceUser(User user)
    {
        var options = new FindOneAndReplaceOptions<User> { IsUpsert = true, };
        userCollection.FindOneAndReplace(Filters.GetFindUserByMailFilter(user.Email), user, options);
    }

    public User FindUser(string email) => userCollection.Find(Filters.GetFindUserByMailFilter(email)).FirstOrDefault();
}