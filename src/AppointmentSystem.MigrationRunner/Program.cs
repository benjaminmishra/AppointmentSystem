using AppointmentSystem.Migration;
using AppointmentSystem.MigrationRunner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// Bind configuration
var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var databaseOptions = new DatabaseOptions();

configuration.GetSection(DatabaseOptions.Section).Bind(databaseOptions);

var options = Options.Create(databaseOptions);

// Build connection string
var connectionString = args.FirstOrDefault() ?? BuildConnectionString(options);

// Execute migrations
return DatabaseMigrator.Execute(connectionString);

static string BuildConnectionString(IOptions<DatabaseOptions> databaseOptions)
{
    if (string.IsNullOrWhiteSpace(databaseOptions.Value.Host))
        throw new InvalidDataException($"{nameof(databaseOptions.Value.Host)} is not defined or is empty in the configuration");

    if (string.IsNullOrWhiteSpace(databaseOptions.Value.Name))
        throw new InvalidDataException($"{databaseOptions.Value.Name} is not defined or is empty in the configuration");

    if (string.IsNullOrWhiteSpace(databaseOptions.Value.User))
        throw new InvalidDataException($"{nameof(databaseOptions.Value.User)} is not defined or is empty in the configuration");

    if (string.IsNullOrWhiteSpace(databaseOptions.Value.Password))
        throw new InvalidDataException($"{nameof(databaseOptions.Value.Password)} is not defined or is empty in the configuration");

    return $"Server={databaseOptions.Value.Host};Port={databaseOptions.Value.Port};Database={databaseOptions.Value.Name};User Id={databaseOptions.Value.User};Password={databaseOptions.Value.Password};";
}
