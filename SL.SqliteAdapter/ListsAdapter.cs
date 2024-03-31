
using Dapper;
using SL.Domain.Adapters;
using SL.Domain.Models.Lists;
using SL.SqliteAdapter.Context;

namespace SL.SqliteAdapter;

public class ListsAdapter(DataContext db) : IListsAdapter
{
    private readonly DataContext _db = db ?? throw new ArgumentNullException(nameof(db));

    public async Task CreateList(CreateListRequest request)
    {
        var conn = _db.CreateConnection();

        await conn.ExecuteAsync(@"
            INSERT INTO Lists (Name, Owner_id)
            VALUES (@Name, @OwnerId)
        ", request);
    }
}