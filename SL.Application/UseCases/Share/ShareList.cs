using SL.Application.Ports.Share;
using SL.Application.UseCases.Interfaces.Share;
using SL.Domain.Adapters;

namespace SL.Application.UseCases.Share;

public class ShareList(
    IShareAdapter shareAdapter,
    IListsAdapter listsAdapter) : IShareList
{
    private readonly IShareAdapter _shareAdapter = shareAdapter ?? throw new ArgumentNullException(nameof(shareAdapter));
    private readonly IListsAdapter _listsAdapter = listsAdapter ?? throw new ArgumentNullException(nameof(listsAdapter));

    public async Task ExecuteAsync(ShareListInput input)
    {
        ValidateEntries(input);

        await ValidateOwnership(input.ListId, input.UserId);

        await _shareAdapter.Create(input.ListId, input.UsersEmail);
    }

    private async Task ValidateOwnership(int listId, int ownerId)
    {
        var list = await _listsAdapter.Find(listId, ownerId);

        if (list is null)
            throw new Exception("List not owned by user.");
    }

    private void ValidateEntries(ShareListInput input)
    {
        if (input.ListId == 0)
            throw new Exception("ListId is required");

        if (input.UserId == 0)
            throw new Exception("UserId is required");

        if (input.UsersEmail == null || !input.UsersEmail.Any())
            throw new Exception("UsersEmail is required");
    }
}
