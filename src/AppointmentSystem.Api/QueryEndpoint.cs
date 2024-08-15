using Dapper;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;

namespace AppointmentSystem.Api;

public class QueryEndpoint : Endpoint<SlotsRequest,Results<Ok<AvailableSlot[]>, NotFound, BadRequest>>
{
    private static string _connectionString = "Server=enpal-coding-challenge-db;Port=5432;Database=coding-challenge;User Id=postgress;Password=mypassword123!;";
    public override void Configure()
    {
        Post("calendar/query");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AvailableSlot[]>, NotFound, BadRequest>> ExecuteAsync(SlotsRequest request, CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", out var day))
            return TypedResults.BadRequest();
        
        var result = await GetAvailableSlots([request.Language], request.Products, [request.Rating]);

        var availableSlots = result as AvailableSlot[] ?? result.ToArray();
        
        if (!availableSlots.Any())
            return TypedResults.NotFound();
        
        return TypedResults.Ok(availableSlots);
    }
    
    private async Task<IEnumerable<AvailableSlot>> GetAvailableSlots(string[] languages, string[] products, string[] ratings)
    {
        await using var dbConnection = new NpgsqlConnection(_connectionString);
        var sql = """
                      SELECT * 
                      FROM fn_matching_available_slots(@langs, @Ratings, @Prods);
                  """;
        
        return await dbConnection.QueryAsync<AvailableSlot>(sql,new
        {
            Langs = languages,
            Ratings = ratings,
            Prods = products
        });
    }
}