using AppointmentSystem.Migrations;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;


// Setup configuration
var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var databaseSettings = new DatabaseOptions();
configuration.GetSection(DatabaseOptions.Section).Bind(databaseSettings);

var options = Options.Create(databaseSettings);

var connectionString = args.FirstOrDefault() ?? BuildConnectionString(options);

// Ensure database is created
EnsureDatabase.For.PostgresqlDatabase(connectionString);

var upgrader =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

// apply upgrades if needed
if (!upgrader.IsUpgradeRequired())
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Database already up-to date!");
    Console.ResetColor();
    return 0;
}

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
return 0;


static string BuildConnectionString(IOptions<DatabaseOptions> databaseOptions) =>
    $"Server={databaseOptions.Value.Host};Port={databaseOptions.Value.Port};Database={databaseOptions.Value.Name};User Id={databaseOptions.Value.User};Password={databaseOptions.Value.Password};";
