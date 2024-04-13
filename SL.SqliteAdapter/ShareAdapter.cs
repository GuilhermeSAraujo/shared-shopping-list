using Dapper;
using SL.Domain.Adapters;
using SL.SqliteAdapter.Context;

namespace SL.SqliteAdapter;
public class ShareAdapter(DataContext db) : IShareAdapter
{
    private readonly DataContext _db = db ?? throw new ArgumentNullException(nameof(db));

    public async Task Create(int listId, IEnumerable<string> emails)
    {
        var insert = @"
            INSERT INTO Share (List_id, User_email)
            VALUES (@ListId, @UserEmail)
        ";

        using var conn = _db.CreateConnection();

        await conn.ExecuteAsync(insert, emails.Select(email => new { ListId = listId, UserEmail = email }));
    }
}