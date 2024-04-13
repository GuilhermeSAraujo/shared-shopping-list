namespace SL.Domain.Adapters;

public interface IShareAdapter
{
    Task Create(int listId, IEnumerable<string> emails);
}