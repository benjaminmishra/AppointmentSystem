using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentSystem.Infrastructure;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {

        services.AddOptions<DatabaseOptions>(config.GetSection(DatabaseOptions.Section))


        services.AddScoped<ICalendarQueryRepository, CalendarQueryRepository>();
        return services;
    }
}