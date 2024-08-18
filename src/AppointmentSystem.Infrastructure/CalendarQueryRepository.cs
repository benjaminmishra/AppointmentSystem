using Dapper;
using Npgsql;
using AppointmentSystem.Domain;

namespace AppointmentSystem.Infrastructure;

public class CalendarQueryRepository : ICalendarQueryRepository
{
    private static string _connectionString = "Server=enpal-coding-challenge-db;Port=5432;Database=coding-challenge;User Id=postgress;Password=mypassword123!;";
    public async Task<IEnumerable<AvailableSlot>> GetAvaiableSlotsAsync(string language, string[] products, string customerRating, CancellationToken cancellationToken)
    {
        using var dbConnection = new NpgsqlConnection(_connectionString);
        
        var sql = """
                  SELECT
                      SlotId,
                      COUNT(DISTINCT SalesManagerId) AS AvailableManagerCout,
                      StartDate
                  FROM
                    vw_available_slots
                  WHERE
                    (Languages && @Langs OR @Langs IS NULL)
                    AND (Products && @Prods OR @Prods IS NULL)
                    AND (CustomerRatings && @Ratings OR @Ratings IS NULL)
                  GROUP BY
                    SlotId, StartDate;
                  """;

        var command = new CommandDefinition(sql, new
        {
            Langs = language,
            Ratings = customerRating,
            Prods = products
        }, cancellationToken: cancellationToken);

        return await dbConnection.QueryAsync<AvailableSlot>(command);
    }
}