using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Foundation.Models;

public class User
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public List<Integration> Integrations { get; set; }
    public string Email { get; set; }

    public User()
    {
        Integrations = new();
    }
}