using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SL.Application.Ports.User;
using SL.Application.UseCases.Interfaces.User;
using SL.WebApi.Dtos.User;

namespace SL.WebApi.Routes.V1;

public static class UserRoutes
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder v1)
    {
        var user = v1.MapGroup("user").WithTags("User");

        user.MapPost("register", PostRegisterUser);
        user.MapPost("auth", PostAuthUser);
    }

    private async static Task<Created> PostRegisterUser(
        [FromBody] RegisterUserRequest request, IRegisterUser registerUser)
    {
        await registerUser.ExecuteAsync(new()
        {
            Email = request.Email,
            Name = request.Name,
            Password = request.Password,
        });

        return TypedResults.Created();
    }

    private async static Task<Results<Ok<AuthUserOutput>, UnauthorizedHttpResult>> PostAuthUser(
        [FromBody] AuthUserRequest request, IAuthUser authUser)
    {
        var result = await authUser.ExecuteAsync(new()
        {
            Email = request.Email,
            Password = request.Password
        });

        if (result is null)
            return TypedResults.Unauthorized();

        return TypedResults.Ok(result);
    }
}