using SL.Domain.Models.Lists;

namespace SL.Domain.Adapters;

public interface IListsAdapter
{
    Task CreateList(CreateListRequest request);
}
