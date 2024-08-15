using Dapper;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;

namespace AppointmentSystem.Api.Endpoints.Calendar;

public class QueryEndpoint : Endpoint<QueryRequest,Results<Ok<AvailableSlot[]>, NotFound, BadRequest>>
{
    private static string _connectionString = "Server=enpal-coding-challenge-db;Port=5432;Database=coding-challenge;User Id=postgress;Password=mypassword123!;";
    public override void Configure()
    {
        Post("calendar/query");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AvailableSlot[]>, NotFound, BadRequest>> ExecuteAsync(QueryRequest request, CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", out var day))
            return TypedResults.BadRequest();
        
        var result = await GetAvailableSlots([request.Language], request.Products, [request.CustomerRating], cancellationToken);

        var availableSlots = result as AvailableSlot[] ?? result.ToArray();
        
        if (!availableSlots.Any())
            return TypedResults.NotFound();
        
        return TypedResults.Ok(availableSlots);
    }
}