using SL.Domain.Models.Users;

namespace SL.WebApi.Middlewares;

public class LoggedUserMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, LoggedUser loggedUser)
    {
        var user = context.User;
        loggedUser.Id = int.TryParse(user.FindFirst("id")?.Value, out int userId) ? userId : 0;
        loggedUser.Name = user.FindFirst("name")?.Value;
        loggedUser.Email = user.FindFirst("email")?.Value;

        await _next(context);
    }
}