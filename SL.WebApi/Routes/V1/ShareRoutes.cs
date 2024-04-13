using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SL.Application.UseCases.Interfaces.Share;
using SL.Domain.Models.Users;
using SL.WebApi.Dtos.Share;

namespace SL.WebApi.Routes.V1;

public static class ShareRoutes
{
    public static void RegisterShareRoutes(this IEndpointRouteBuilder v1)
    {
        var user = v1.MapGroup("share").WithTags("Share").RequireAuthorization();

        user.MapPost("{listId}", PostShareList);
    }

    public async static Task<NoContent> PostShareList(
        LoggedUser loggedUser,
        [FromRoute] int listId,
        [FromBody] ShareListRequest request,
        IShareList shareList)
    {
        await shareList.ExecuteAsync(new()
        {
            ListId = listId,
            UserId = loggedUser.Id,
            UsersEmail = request.UsersEmail
        });

        return TypedResults.NoContent();
    }
}