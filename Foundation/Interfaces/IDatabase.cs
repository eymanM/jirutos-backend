namespace Foundation.Interfaces;

public interface IDatabase
{
    void InsertOrReplaceUser(User user);

    User FindUser(string email);
    void DeletAllWithField(string fieldName);
}