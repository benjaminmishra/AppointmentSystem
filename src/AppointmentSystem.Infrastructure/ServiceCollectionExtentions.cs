using AppointmentSystem.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentSystem.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<DatabaseOptions> configAction)
    {
        services.Configure(configAction);
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ICalendarQueryRepository, CalendarQueryRepository>();
        return services;
    }
}