using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace AppointmentSystem.Infrastructure;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IOptions<DatabaseOptions> _databaseOptions;

    public DbConnectionFactory(IOptions<DatabaseOptions> options)
    {
        _databaseOptions = options;
    }

    public IDbConnection CreateConnection()
    {
        var connStr = $"Server={_databaseOptions.Value.Host};Port={_databaseOptions.Value.Port};Database={_databaseOptions.Value.Name};User Id={_databaseOptions.Value.User};Password={_databaseOptions.Value.Password};";
        var conn = new NpgsqlConnection(connStr);
        conn.Open();
        return conn;
    }
}