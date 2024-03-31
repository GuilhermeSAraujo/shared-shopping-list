using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SL.Domain.Models.Users;
using SL.WebApi.Dtos.Item;

namespace SL.WebApi.Routes.V1;

public static class ItemRoutes
{
    public static void RegisterItemRoutes(this IEndpointRouteBuilder v1)
    {
        var item = v1.MapGroup("item").WithTags("Items").RequireAuthorization();

        item.MapPost("", PostCreateItem);
    }

    private static async Task<Created> PostCreateItem(
        LoggedUser loggedUser,
        [FromBody] CreateItemRequest request,
        ICreateItem createItem)
    {
        await createItem.ExecuteAsync(new()
        {
            ListId = request.ListId,
            UserId = loggedUser.Id,
            Name = request.Name
        });

        return TypedResults.Created();
    }
}
