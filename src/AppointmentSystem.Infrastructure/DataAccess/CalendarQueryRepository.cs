using AppointmentSystem.Domain;
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
                      SlotId,
                      COUNT(DISTINCT SalesManagerId) AS AvailableManagerCout,
                      StartDate
                  FROM
                    vw_available_slots
                  WHERE
                    (Languages && @Langs OR @Langs IS NULL)
                    AND (Products && @Prods OR @Prods IS NULL)
                    AND (CustomerRatings && @Ratings OR @Ratings IS NULL)
                    AND (DATE(StartDate) = @FilterDate OR @FilterDate IS NULL)  
                  GROUP BY
                    SlotId, StartDate;
                  """;

        var command = new CommandDefinition(sql, new
        {
            Langs = language,
            Ratings = customerRating,
            Prods = products,
            FilterDate = filterDate,
        }, cancellationToken: cancellationToken);

        return await dbConnection.QueryAsync<AvailableSlot>(command);
    }
}