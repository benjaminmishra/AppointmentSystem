using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace AppointmentSystem.Infrastructure;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddOptions<DatabaseOptions>(config.GetSection("DB"))
        
        services.AddScoped<ICalendarQueryRepository, CalendarQueryRepository>();
        return services;
    }
}