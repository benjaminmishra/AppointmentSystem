using System.Collections;
using System.Data;
using Npgsql;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Api;

public static class Endpoints
{
    private static string _connectionString = "Server=enpal-coding-challenge-db;Port=5432;Database=coding-challenge;User Id=postgress;Password=mypassword123!;";
    public static void RegisterEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var calendarGroup = endpointRouteBuilder.MapGroup("/calendar");

        calendarGroup
        .MapPost("query",QueryHandler)
        .WithName("Query")
        .WithOpenApi();
    }

    private static IEnumerable<AvailableSlot> QueryHandler(SlotsRequest request)
    {
        return GetAvailableSlots([request.Language], request.Products, [request.Rating]);
    }
    
    private static IEnumerable<AvailableSlot> GetAvailableSlots(string[] languages, string[] products, string[] ratings)
    {
        using IDbConnection dbConnection = new NpgsqlConnection(_connectionString);
        var sql = $"""
                   SELECT
                   slot_id,
                   start_date,
                   end_date,
                   sales_manager_id,
                   sales_manager_name,
                   languages,
                   products,
                   customer_ratings
                   FROM vw_matching_available_slots
                   WHERE
                   -- Filter based on languages
                   EXISTS (
                       SELECT 1
                       FROM unnest(languages) AS lang
                       WHERE lang = ANY(@Languages)
                   )
                   -- Filter based on customer ratings
                   AND EXISTS (
                       SELECT 1
                       FROM unnest(customer_ratings) AS rate
                       WHERE rate = ANY(@Ratings)
                   )
                   -- Filter based on products
                   AND EXISTS (
                       SELECT 1
                       FROM unnest(products) AS prod
                       WHERE prod = ANY(@Products)
                   );
                   """;

        return dbConnection.Query<AvailableSlot>(sql, new
        {
            Languages = languages, 
            Products = products, 
            Ratings = ratings
        });
    }

}
