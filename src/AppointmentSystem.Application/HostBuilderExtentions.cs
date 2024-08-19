using AppointmentSystem.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentSystem.Application;

public static class HostBuilderExtentions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGetAvailableSlotsQueryHandler, GetAvailableSlotsQueryHandler>();
        return services;
    }
}