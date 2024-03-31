
using SL.WebApi.Routes.V1;

namespace SL.WebApi.Routes;

public static class Router
{
    public static void RegisterRoutes(this WebApplication app)
    {
        var api = app.MapGroup("api");
        var v1 = api.MapGroup("v1");

        RegisterV1Routes(v1);
    }

    private static void RegisterV1Routes(RouteGroupBuilder v1)
    {
        v1.RegisterUserRoutes();
        v1.RegisterListRoutes();
        v1.RegisterShareRoutes();
    }
}