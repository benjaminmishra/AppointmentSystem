using AppointmentSystem.Domain.Models;
using Dapper;

namespace AppointmentSystem.Infrastructure.DataAccess;

public class CalendarQueryRepository : ICalendarQueryRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CalendarQueryRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<AvailableSlot>> GetAvailableSlotsAsync(
        string language,
        string[] products,
        string customerRating,
        DateTime filterDate,
        CancellationToken cancellationToken)
    {
        using var dbConnection = _dbConnectionFactory.CreateConnection();

        var sql = """
                                          SELECT
                                              COUNT(DISTINCT SalesManagerId) AS AvailableCount,
                                              StartDate
                                          FROM
                                              vw_available_slots
                                          WHERE
                                              @Lang = ANY(Languages)
                                              AND @Rating = ANY(CustomerRatings)
                                              AND Products @> @Prods::varchar[]
                                              AND (DATE(StartDate) = DATE(@FilterDate))
                                          GROUP BY
                                              StartDate;
                  """;

        var command = new CommandDefinition(sql, new
        {
            Lang = language,
            Rating = customerRating,
            Prods = products,
            FilterDate = filterDate,
        }, cancellationToken: cancellationToken);

        return await dbConnection.QueryAsync<AvailableSlot>(command);
    }
}