using Microsoft.Extensions.DependencyInjection;
using SL.Application.UseCases.Interfaces.List;
using SL.Application.UseCases.Interfaces.Share;
using SL.Application.UseCases.Interfaces.User;
using SL.Application.UseCases.List;
using SL.Application.UseCases.Share;
using SL.Application.UseCases.User;

namespace SL.Application.Microsoft.Extensions.DependencyInjection;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<IRegisterUser, RegisterUser>();
        services.AddSingleton<IAuthUser, AuthUser>();

        services.AddSingleton<ICreateList, CreateList>();

        services.AddSingleton<IShareList, ShareList>();

        return services;
    }
}

