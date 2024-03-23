using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SL.Application.UseCases.Interfaces;
using SL.WebApi.Dtos.User;

namespace SL.WebApi.Routes.V1;

public static class UserRoutes
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder v1)
    {
        var acessoTemporario = v1.MapGroup("user").WithTags("User");

        acessoTemporario.MapPost("register", PostRegisterUser);
    }

    private async static Task<Ok> PostRegisterUser([FromBody] RegisterUserRequest request, IRegisterUser registerUser)
    {
        await registerUser.ExecuteAsync(new()
        {
            Email = request.Email,
            Name = request.Name,
            Password = request.Password,
        });

        return TypedResults.Ok();
    }
}