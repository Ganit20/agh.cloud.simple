using cloud.core.objects.Model;
using RestEase;

namespace cloud.core.database.interf;
[BasePath("api/user")]
public interface IDbUserApi
{
    public const string GetUserByIdPath = "user/{id}";
    [Get(GetUserByIdPath)]
    public Task<User> GetUserById([Path] int id);
    public const string GetUserByMailPath = "mail/user/{email}";
    [Get(GetUserByMailPath)]
    public Task<User> GetUserByMail([Path] string email);
    public const string PostUser = "user";

    [Post(PostUser)]

    public Task<User> Adduser([Body] AddUserRequest request);
    public const string PutSaveRefreshTokenPath = "{refreshToken}/{id}";
    [Put(PutSaveRefreshTokenPath)]
    public Task PutSaveRefreshToken([Path] string refreshToken, [Path] int id);

}


