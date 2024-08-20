using AppointmentSystem.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using AppointmentSystem.MigrationRunner;

// Bind configuration
var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var databaseSettings = new DatabaseOptions();
configuration.GetSection(DatabaseOptions.Section).Bind(databaseSettings);

var options = Options.Create(databaseSettings);

// Build connection string
var connectionString = args.FirstOrDefault() ?? BuildConnectionString(options);

// Execute migrations
return DatabaseMigrator.Execute(connectionString);

static string BuildConnectionString(IOptions<DatabaseOptions> databaseOptions) =>
    $"Server={databaseOptions.Value.Host};Port={databaseOptions.Value.Port};Database={databaseOptions.Value.Name};User Id={databaseOptions.Value.User};Password={databaseOptions.Value.Password};";
