using System.Reflection;
using DbUp;

var connectionString =
    args.FirstOrDefault()
    ?? "Server=enpal-coding-challenge-db;Port=5432;Database=coding-challenge;User Id=postgress;Password=mypassword123!;";

EnsureDatabase.For.PostgresqlDatabase(connectionString);

var upgrader =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

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