using AppointmentSystem.Infrastructure;
using AppointmentSystem.Migration;
using Microsoft.Extensions.Options;
using Testcontainers.PostgreSql;

namespace AppointmentSystem.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private const string DatabaseName = "testdb";
    private const string UserName = "postgres";
    private const string Password = "password";
    private const int Port = 5432;

    private readonly PostgreSqlContainer _postgreSqlContainer;

    public IDbConnectionFactory? DbConnectionFactory { get; set; }

    public DatabaseFixture()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase(DatabaseName)
            .WithUsername(UserName)
            .WithPassword(Password)
            .WithPortBinding(Port)
            .Build();
    }

    public async Task InitializeAsync()
    {
        // Start the PostgreSQL container, wait upto 1 min to launch the container
        await _postgreSqlContainer.StartAsync(TestHelpers.CreateCancellationToken(60000));
        var connectionString = _postgreSqlContainer.GetConnectionString();
        var databaseOptions = new DatabaseOptions
        {
            Host = _postgreSqlContainer.Hostname,
            Name = DatabaseName,
            User = UserName,
            Password = Password,
            Port = Port
        };

        DbConnectionFactory = new DbConnectionFactory(Options.Create(databaseOptions));

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
