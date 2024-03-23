using Dapper;
using SL.Domain.Adapters;
using SL.Domain.Models.Users;
using SL.SqliteAdapter.Context;

namespace SL.SqliteAdapter;

public class UsersAdapter(DataContext db) : IUsersAdapter
{
    private readonly DataContext _db = db ?? throw new ArgumentNullException(nameof(db));

    public async Task CreateUser(CreateUserRequest request)
    {
        var conn = _db.CreateConnection();

        await conn.ExecuteAsync(@"
            INSERT INTO Users (Name, Email, Password)
            VALUES (@Name, @Email, @Password)
        ", request);
    }

    public async Task<UserModel?> FindUserByEmail(string email)
    {
        var conn = _db.CreateConnection();

        var result = await conn.QuerySingleOrDefaultAsync<UserModel>(@"
            SELECT Id, Email, Name, Password 
            FROM Users
            WHERE Email = @email", new { email });

        return result;
    }
}
