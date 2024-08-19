using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentSystem.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {

        services.AddOptions<DatabaseOptions>().Bind(config.GetSection());


        services.AddScoped<ICalendarQueryRepository, CalendarQueryRepository>();
        return services;
    }
}