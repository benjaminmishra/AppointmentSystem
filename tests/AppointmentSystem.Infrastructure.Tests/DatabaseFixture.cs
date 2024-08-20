using AppointmentSystem.Migrations;
using Testcontainers.PostgreSql;

namespace AppointmentSystem.Infrastructure.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public DatabaseFixture()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();
    }

    public async Task InitializeAsync()
    {
        // Start the PostgreSQL container
        await _postgreSqlContainer.StartAsync();

        var connectionString = _postgreSqlContainer.GetConnectionString();

        // Run migrations
        var result = DatabaseMigrator.Execute(connectionString);
        if (result != 0)
            throw new Exception("Database migration failed.");
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }
}
