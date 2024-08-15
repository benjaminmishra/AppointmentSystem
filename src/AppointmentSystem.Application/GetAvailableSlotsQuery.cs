namespace AppointmentSystem.Application;

public class GetAvailableSlotsQuery
{
    private async Task<IEnumerable<AvailableSlot>> GetAvailableSlots(string[] languages, string[] products, string[] ratings, CancellationToken cancellationToken)
    {
        await using var dbConnection = new NpgsqlConnection(_connectionString);
        var sql = """
                      SELECT * 
                      FROM fn_matching_available_slots(@langs, @Ratings, @Prods);
                  """;
        var command = new CommandDefinition(sql, new
        {
            Langs = languages,
            Ratings = ratings,
            Prods = products
        }, cancellationToken: cancellationToken);
        
        return await dbConnection.QueryAsync<AvailableSlot>(command);
    }
}