using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SL.Application.UseCases.Interfaces.List;
using SL.Domain.Models.Users;

namespace SL.WebApi.Routes.V1;

public static class ListRoutes
{
    public static void RegisterListRoutes(this IEndpointRouteBuilder v1)
    {
        var user = v1.MapGroup("list").WithTags("List").RequireAuthorization();

        user.MapPost("{listName}", PostCreateList);
    }

    public async static Task<Created> PostCreateList(
        LoggedUser loggedUser, [FromRoute] string listName, ICreateList createList)
    {
        await createList.ExecuteAsync(new()
        {
            Name = listName,
            UserId = loggedUser.Id
        });

        return TypedResults.Created();
    }
}