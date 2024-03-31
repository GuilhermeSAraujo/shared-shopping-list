using System.Text;
using SL.Domain.Adapters;
using SL.SqliteAdapter.Context;

namespace SL.SqliteAdapter;
public class ShareAdapter(DataContext db) : IShareAdapter
{
    private readonly DataContext _db = db ?? throw new ArgumentNullException(nameof(db));

    public async Task Create(int listId, IEnumerable<string> emails)
    {
        var values = new StringBuilder(@"
            INSERT INTO Share (List_id, User_id)
            VALUES");

        foreach (var email in emails)
        {
            values.AppendLine($"({listId}, @)");

        }
    }
}