using Microsoft.Extensions.DependencyInjection;
using SL.Domain.Adapters;
using SL.SqliteAdapter.Context;

namespace SL.SqliteAdapter.Microsoft.Extensions.DependencyInjection;

public static class DataAccessCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<DataContext>();

        services.AddSingleton<IUsersAdapter, UsersAdapter>();

        return services;
    }
}