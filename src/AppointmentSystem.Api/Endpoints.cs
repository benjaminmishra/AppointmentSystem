using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Api;

public static class Endpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var calendarGroup = endpointRouteBuilder.MapGroup("/calendar");

        calendarGroup
        .MapPost("query",QueryHandler)
        .WithName("Query")
        .WithOpenApi();
    }

    private static ActionResult QueryHandler()
    {
        return new OkResult();
    }
}
