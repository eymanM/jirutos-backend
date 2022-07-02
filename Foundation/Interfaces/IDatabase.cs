using Foundation.Models;

namespace Foundation.Interfaces;

public interface IDatabase
{
    void InsertOrReplaceUser(User user);

    User FindUser(string email);
}