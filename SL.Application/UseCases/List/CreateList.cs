using SL.Application.Ports.List;
using SL.Application.UseCases.Interfaces.List;
using SL.Domain.Adapters;

namespace SL.Application.UseCases.List;

public class CreateList(IListsAdapter listsAdapter) : ICreateList
{
    private readonly IListsAdapter _listsAdapter = listsAdapter ?? throw new ArgumentNullException(nameof(listsAdapter));

    public async Task ExecuteAsync(CreateListInput input)
    {
        ValidateEntries(input);

        await _listsAdapter.CreateList(new()
        {
            Name = input.Name,
            OwnerId = input.UserId
        });
    }

    private void ValidateEntries(CreateListInput input)
    {
        if (input.UserId == 0)
            throw new Exception("User id cannot be empty");

        if (string.IsNullOrEmpty(input.Name))
            throw new Exception("List name cannot be empty");
    }
}
