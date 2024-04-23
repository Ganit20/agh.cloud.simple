using cloud.core.objects.Model;
using RestEase;

namespace cloud.core.database.interf;

public interface IDbUserApi 
{
    public const string GetUserByIdPath = "user/{id}";
    [Get(GetUserByIdPath)]
    public Task<User> GetUserById([Path] int id);
}

